using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotDll.Model.Data;
using DotDll.Model.Data.Base;
using DotDll.Model.Data.Members;
using Type = System.Type;

// ReSharper disable MemberCanBeMadeStatic.Local

namespace DotDll.Model.Analysis
{
    public class ReflectionDllAnalyzer : IDllAnalyzer
    {
        public delegate Assembly AssemblyProvider(string path);

        private const BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance |
                                           BindingFlags.Static;

        private readonly AssemblyProvider _provider;
        private readonly Dictionary<Type, Data.Type> _typesMapping = new Dictionary<Type, Data.Type>();

        public ReflectionDllAnalyzer(AssemblyProvider provider)
        {
            _provider = provider;
        }

        public MetadataInfo Analyze(string path)
        {
            Assembly assembly = _provider(path);

            _typesMapping.Clear();

            var metadataInfo = new MetadataInfo(assembly.GetName().Name);

            var namespaces = assembly
                .GetTypes()
                .GroupBy(type => type.Namespace);


            foreach (var ns in namespaces)
            {
                var analyzedNamespace = AnalyzeNamespace(ns.Key, ns.ToList());

                metadataInfo.AddNamespace(analyzedNamespace);
            }

            return metadataInfo;
        }

        private Namespace AnalyzeNamespace(string namespaceName, IEnumerable<Type> namespaceTypes)
        {
            var nSpace = new Namespace(namespaceName);

            foreach (var systemType in namespaceTypes)
            {
                var type = AnalyzeType(systemType);
                nSpace.AddType(type);
            }

            return nSpace;
        }

        private Data.Type AnalyzeType(Type systemType)
        {
            if (_typesMapping.ContainsKey(systemType)) return _typesMapping[systemType];

            var type = new Data.Type(
                systemType.Name,
                AnalyzeTypeAccessLevel(systemType),
                AnalyzeTypeKind(systemType),
                systemType.IsSealed,
                systemType.IsAbstract,
                new List<Member>(),
                AnalyzeGenericArguments(systemType.GetGenericArguments())
            );

            _typesMapping[systemType] = type;

            AnalyzeTypeMembers(type, systemType);

            return type;
        }

        private void AnalyzeTypeMembers(Data.Type type, Type systemType)
        {
            AnalyzeTypeConstructor(type, systemType);
            AnalyzeTypeField(type, systemType);
            AnalyzeTypeProperties(type, systemType);
            AnalyzeTypeEvents(type, systemType);
            AnalyzeTypeMethods(type, systemType);
            AnalyzeNestedType(type, systemType);
        }

        private void AnalyzeTypeConstructor(Data.Type type, Type systemType)
        {
            foreach (var constructorInfo in systemType.GetConstructors(Flags))
                type.AddMember(new Constructor(
                    type,
                    AnalyzeAccessLevel(constructorInfo),
                    AnalyzeMethodParameters(constructorInfo.GetParameters())
                ));
        }

        private void AnalyzeTypeField(Data.Type type, IReflect systemType)
        {
            foreach (var fieldInfo in systemType.GetFields(Flags))
            {
                Field.Constraint constraint;

                if (fieldInfo.IsLiteral)
                    constraint = Field.Constraint.Const;
                else if (fieldInfo.IsInitOnly)
                    constraint = Field.Constraint.ReadOnly;
                else
                    constraint = Field.Constraint.None;

                var field = new Field(
                    fieldInfo.Name,
                    AnalyzeAccessLevel(fieldInfo),
                    AnalyzeType(fieldInfo.FieldType),
                    fieldInfo.IsStatic,
                    constraint
                );
                type.AddMember(field);
            }
        }

        private Access AnalyzeAccessLevel(dynamic info)
        {
            if (info.IsPublic) return Access.Public;

            if (info.IsFamilyAndAssembly) return Access.InternalProtected;

            if (info.IsFamily) return Access.Protected;

            return info.IsAssembly ? Access.Internal : Access.Private;
        }


        private void AnalyzeTypeEvents(Data.Type type, Type systemType)
        {
            foreach (var eventInfo in systemType.GetEvents(Flags))
            {
                var eve = new Event(
                    eventInfo.Name,
                    AnalyzeMethod(eventInfo.RemoveMethod),
                    AnalyzeMethod(eventInfo.AddMethod),
                    AnalyzeMethod(eventInfo.RaiseMethod)
                );

                type.AddMember(eve);
            }
        }


        private void AnalyzeNestedType(Data.Type type, Type systemType)
        {
            foreach (var nested in systemType.GetNestedTypes(Flags))
                type.AddMember(new NestedType(AnalyzeType(nested)));
        }


        private void AnalyzeTypeProperties(Data.Type type, IReflect systemType)
        {
            foreach (var propertyInfo in systemType.GetProperties(Flags))
            {
                var getter = propertyInfo.GetMethod;
                var setter = propertyInfo.SetMethod;

                var property = new Property(propertyInfo.Name, AnalyzeMethod(getter), AnalyzeMethod(setter));
                type.AddMember(property);
            }
        }

        private void AnalyzeTypeMethods(Data.Type type, IReflect systemType)
        {
            foreach (var methodInfo in systemType.GetMethods(Flags))
            {
                var method = AnalyzeMethod(methodInfo);

                if (method != null) type.AddMember(method);
            }
        }

        private Method? AnalyzeMethod(MethodInfo? methodInfo)
        {
            if (methodInfo == null) return null;

            var methodBuilder = new Method.Builder(methodInfo.Name, AnalyzeAccessLevel(methodInfo));

            var method = methodBuilder
                .WithAbstract(methodInfo.IsAbstract)
                .WithSealed(methodInfo.IsFinal)
                .WithStatic(methodInfo.IsStatic)
                .WithVirtual(methodInfo.IsVirtual)
                .WithReturnType(AnalyzeType(methodInfo.ReturnType))
                .WithParameters(AnalyzeMethodParameters(methodInfo.GetParameters()))
                .WithGenericArguments(AnalyzeGenericArguments(methodInfo.GetGenericArguments()))
                .Build();

            return method;
        }

        private List<Parameter> AnalyzeMethodParameters(IEnumerable<ParameterInfo> parameters)
        {
            return parameters.Select(param => new Parameter(param.Name, AnalyzeType(param.ParameterType)))
                .ToList();
        }

        private List<Data.Type> AnalyzeGenericArguments(IEnumerable<Type> genericArguments)
        {
            return genericArguments.Select(AnalyzeType).ToList();
        }

        private Data.Type.Kind AnalyzeTypeKind(Type type)
        {
            if (type.IsArray) return Data.Type.Kind.Array;

            if (type.IsEnum) return Data.Type.Kind.Enum;

            if (type.IsInterface) return Data.Type.Kind.Interface;

            return type.IsGenericParameter ? Data.Type.Kind.GenericArg : Data.Type.Kind.Class;
        }

        private Access AnalyzeTypeAccessLevel(Type type)
        {
            if (type.IsPublic || type.IsNestedPublic) return Access.Public;

            if (type.IsNestedFamANDAssem) return Access.InternalProtected;

            if (type.IsNestedFamily) return Access.Protected;

            return type.IsNestedAssembly ? Access.Internal : Access.Private;
        }
    }
}