using System;
using System.Reflection;
using System.Reflection.Emit;
// ReSharper disable MemberCanBeMadeStatic.Local

namespace DotDll.Tests.Model.Analysis
{
    /// 
    /// TestAssemblyGenerator generates assembly with 2 namespaces { Namespace1, Namespace2 }
    /// namespace Namespace1
    /// {
    ///     class TestClass
    ///     {
    ///         public virtual string PublicVirtualMethod(int index, string str) {}
    ///
    ///         private sealed string PrivateSealed(int) {}
    ///
    ///         protected const string ConstProtectedField;
    ///
    ///         protected internal object ProtectedInternalField;
    ///
    ///         internal readonly int ReadOnlyInternalField;
    ///
    ///         public static int StaticField;
    ///
    ///         public event Action TestEvent;
    ///
    ///         public int TestProperty { get; set; }
    ///     }
    ///
    ///     public interface Interface
    ///     {
    ///        public string AbstractMethod(int);
    ///
    ///        public class PublicNestedType{}
    /// 
    ///        protected class ProtectedNestedType{}
    /// 
    ///        protected internal class ProtectedInternalNestedType{}
    /// 
    ///        internal class InternalNestedType{}
    /// 
    ///        private class PrivateNestedType{}
    ///     }
    ///
    ///     public static class StaticClass
    ///     {
    ///     }
    /// }
    ///
    /// namespace Namespace2
    /// {
    ///     public enum EmptyEnum
    ///     {
    ///     }
    /// }
    ///
    public class TestAssemblyGenerator
    {
        public Assembly Generate()
        {
            var builder = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("Test.Assembly"), AssemblyBuilderAccess.ReflectionOnly
            );

            var moduleBuilder = builder.DefineDynamicModule("Test.Assembly.Module");

            DefineTestClass("Namespace1", moduleBuilder);

            DefineEmptyEnum("Namespace2", moduleBuilder);

            DefineInterface("Namespace1", moduleBuilder);

            DefineStaticClass("Namespace1", moduleBuilder);

            return builder;
        }

        private void DefineStaticClass(string nSpace, ModuleBuilder moduleBuilder)
        {
            const TypeAttributes attr = 
                TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.Public;

            moduleBuilder.DefineType($"{nSpace}.StaticClass", attr).CreateType();
        }

        private void DefineInterface(string nSpace, ModuleBuilder moduleBuilder)
        {
            const TypeAttributes attrs = 
                TypeAttributes.Interface | TypeAttributes.Abstract | TypeAttributes.Public;

            var typeBuilder = moduleBuilder
                .DefineType($"{nSpace}.Interface", attrs);

            typeBuilder.DefineMethod(
                "AbstractMethod",
                MethodAttributes.Public | MethodAttributes.Abstract | MethodAttributes.Virtual,
                CallingConventions.Standard,
                typeof(string),
                new[] {typeof(int)}
            );

            typeBuilder
                .DefineNestedType("PublicNestedType", TypeAttributes.Class | TypeAttributes.NestedPublic)
                .CreateType();

            typeBuilder
                .DefineNestedType("ProtectedNestedType", TypeAttributes.Class | TypeAttributes.NestedFamily)
                .CreateType();

            typeBuilder
                .DefineNestedType("InternalNestedType", TypeAttributes.Class | TypeAttributes.NestedAssembly)
                .CreateType();

            typeBuilder
                .DefineNestedType(
                    "ProtectedInternalNestedType",
                    TypeAttributes.Class | TypeAttributes.NestedFamANDAssem
                )
                .CreateType();

            typeBuilder
                .DefineNestedType("PrivateNestedType", TypeAttributes.Class | TypeAttributes.NestedPrivate)
                .CreateType();

            typeBuilder.CreateType();
        }

        private void DefineEmptyEnum(string nSpace, ModuleBuilder moduleBuilder)
        {
            moduleBuilder
                .DefineEnum($"{nSpace}.EmptyEnum", TypeAttributes.NotPublic, typeof(int))
                .CreateType();
        }

        private void DefineTestClass(string nSpace, ModuleBuilder moduleBuilder)
        {
            var typeBuilder = moduleBuilder
                .DefineType($"{nSpace}.TestClass");

            var publicVirtualMethodBuilder = typeBuilder.DefineMethod(
                "PublicVirtualMethod",
                MethodAttributes.Public | MethodAttributes.Virtual,
                CallingConventions.Standard,
                typeof(string),
                new[] {typeof(int), typeof(string)}
            );

            publicVirtualMethodBuilder.DefineParameter(1, ParameterAttributes.None, "index");

            publicVirtualMethodBuilder.DefineParameter(2, ParameterAttributes.None, "str");

            publicVirtualMethodBuilder.GetILGenerator().Emit(OpCodes.Ret);

            var privateSealedMethod = typeBuilder.DefineMethod(
                "PrivateSealedMethod",
                MethodAttributes.Private | MethodAttributes.Final,
                CallingConventions.Standard,
                typeof(string),
                new[] {typeof(int)}
            );

            privateSealedMethod.GetILGenerator().Emit(OpCodes.Ret);

            typeBuilder.DefineField("ConstProtectedField", typeof(string),
                FieldAttributes.Family | FieldAttributes.Literal);
            typeBuilder.DefineField("ProtectedInternalField", typeof(object), FieldAttributes.FamANDAssem);
            typeBuilder.DefineField("ReadOnlyInternalField", typeof(int),
                FieldAttributes.Assembly | FieldAttributes.InitOnly);
            typeBuilder.DefineField("StaticField", typeof(int), FieldAttributes.Static | FieldAttributes.Public);

            DefineEvent(typeBuilder);

            DefineProperty(typeBuilder);

            typeBuilder.CreateType();
        }

        private void DefineProperty(TypeBuilder typeBuilder)
        {
            var propertyBuilder = typeBuilder.DefineProperty(
                "TestProperty",
                PropertyAttributes.None,
                CallingConventions.Any,
                typeof(int),
                null
            );

            var getMethod = typeBuilder.DefineMethod(
                "_getTestProperty",
                MethodAttributes.Public,
                typeof(int),
                null
            );

            getMethod.GetILGenerator().Emit(OpCodes.Ret);

            var setMethod = typeBuilder.DefineMethod(
                "_setTestProperty",
                MethodAttributes.Public,
                typeof(void),
                new[] {typeof(int)}
            );

            setMethod.GetILGenerator().Emit(OpCodes.Ret);

            propertyBuilder.SetSetMethod(setMethod);
            propertyBuilder.SetGetMethod(getMethod);
        }

        private void DefineEvent(TypeBuilder typeBuilder)
        {
            var eventBuilder = typeBuilder.DefineEvent("TestEvent", EventAttributes.None, typeof(Action));

            var addMethod = typeBuilder.DefineMethod(
                "_addEvent",
                MethodAttributes.Private,
                CallingConventions.Standard,
                typeof(void),
                new[] {typeof(Action)}
            );
            addMethod.GetILGenerator().Emit(OpCodes.Ret);

            var removeMethod = typeBuilder.DefineMethod(
                "_removeEvent",
                MethodAttributes.Private,
                CallingConventions.Standard,
                typeof(void),
                new[] {typeof(Action)}
            );
            removeMethod.GetILGenerator().Emit(OpCodes.Ret);

            eventBuilder.SetRemoveOnMethod(removeMethod);
            eventBuilder.SetAddOnMethod(addMethod);
        }
    }
}