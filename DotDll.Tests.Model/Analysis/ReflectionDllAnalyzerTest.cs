using System.Linq;
using DotDll.Model.Analysis;
using DotDll.Model.Data;
using DotDll.Model.Data.Base;
using DotDll.Model.Data.Members;
using NUnit.Framework;

namespace DotDll.Tests.Model.Analysis
{
    [TestFixture]
    public class ReflectionDllAnalyzerTest
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            var generator = new TestAssemblyGenerator();

            _analyzer = new ReflectionDllAnalyzer(path => generator.Generate(), true);

            _metadataInfo = _analyzer.Analyze("");
        }

        private ReflectionDllAnalyzer _analyzer;
        private MetadataInfo _metadataInfo;

        private const int DefaultMembersCount = 7;

        [Test]
        public void Analyze_CheckProperMetadataName()
        {
            var metadataInfo = _analyzer.Analyze("");

            Assert.AreEqual("Test.Assembly", metadataInfo.Name);
        }

        [Test]
        public void Analyze_CheckTypesInNamespaces()
        {
            var namespaces = new[]
            {
                "Namespace1",
                "Namespace2"
            };

            CollectionAssert.AreEqual(
                namespaces,
                _metadataInfo.Namespaces.Select(n => n.Name)
            );

            var namespaceTypes1 = new[]
            {
                "TestClass",
                "Interface",
                "PublicNestedType",
                "ProtectedNestedType",
                "InternalNestedType",
                "ProtectedInternalNestedType",
                "PrivateNestedType",
                "StaticClass"
            };

            var actual1 = _metadataInfo
                .FindNamespace("Namespace1")
                .Types
                .Select(type => type.Name);

            CollectionAssert.AreEqual(
                namespaceTypes1,
                actual1
            );

            var namespaceTypes2 = new[] {"EmptyEnum"};

            var actual2 = _metadataInfo
                .FindNamespace("Namespace2")
                .Types
                .Select(type => type.Name);

            CollectionAssert.AreEqual(
                namespaceTypes2,
                actual2
            );
        }


        [TestCase("Namespace2", "EmptyEnum", Type.Kind.Enum)]
        [TestCase("Namespace1", "Interface", Type.Kind.Interface)]
        [TestCase("Namespace1", "TestClass", Type.Kind.Class)]
        public void Analyzer_CheckTypeKind(string nSpace, string typeName, Type.Kind kind)
        {
            var actual = _metadataInfo.FindType(nSpace, typeName).TypeKind;

            Assert.AreEqual(kind, actual);
        }

        [TestCase("Namespace2", "EmptyEnum", Access.Private)]
        [TestCase("Namespace1", "Interface", Access.Public)]
        public void Analyzer_CheckTypeAccessLevel(string nSpace, string typeName, Access accessLevel)
        {
            var actual = _metadataInfo.FindType(nSpace, typeName).Access;

            Assert.AreEqual(accessLevel, actual);
        }

        [TestCase("Namespace1", "Interface", true, false, false)]
        [TestCase("Namespace1", "StaticClass", true, true, true)]
        [TestCase("Namespace2", "EmptyEnum", false, true, false)]
        public void Analyzer_CheckTypeIsAbstractSealedStatic(
            string nSpace,
            string typeName,
            bool isAbstract,
            bool isSealed,
            bool isStatic
        )
        {
            var type = _metadataInfo.FindType(nSpace, typeName);

            Assert.AreEqual(isAbstract, type.IsAbstract);
            Assert.AreEqual(isSealed, type.IsSealed);
            Assert.AreEqual(isStatic, type.IsStatic);
        }

        [Test]
        public void Analyze_CheckTypeMembersCount()
        {
            var count = _metadataInfo
                .FindType("Namespace1", "TestClass")
                .Members
                .Count;

            Assert.AreEqual(DefaultMembersCount + 12, count);
        }

        [TestCase("Namespace1", "TestClass", "PublicVirtualMethod", Access.Public)]
        [TestCase("Namespace1", "TestClass", "ConstProtectedField", Access.Protected)]
        [TestCase("Namespace1", "TestClass", "ReadOnlyInternalField", Access.Internal)]
        [TestCase("Namespace1", "TestClass", "ProtectedInternalField", Access.InternalProtected)]
        [TestCase("Namespace1", "TestClass", "TestEvent", Access.Inner)]
        [TestCase("Namespace1", "TestClass", "PrivateSealedMethod", Access.Private)]
        public void Analyze_CheckMemberAccess(string nSpace, string typeName, string memberName, Access access)
        {
            var actual = _metadataInfo
                .FindMember(nSpace, typeName, memberName)
                .AccessLevel;

            Assert.AreEqual(access, actual);
        }


        [TestCase("Namespace1", "TestClass", "StaticField", true, false)]
        [TestCase("Namespace1", "TestClass", "PublicVirtualMethod", false, false)]
        [TestCase("Namespace1", "Interface", "AbstractMethod", false, true)]
        public void Analyze_CheckMemberIsStaticAbstractSealed(
            string nSpace,
            string typeName,
            string memberName,
            bool isStatic,
            bool isAbstract
        )
        {
            var member = _metadataInfo.FindMember(nSpace, typeName, memberName);

            Assert.AreEqual(isAbstract, member.IsAbstract);
            Assert.AreEqual(isStatic, member.IsStatic);
        }

        [Test]
        public void Analyze_CheckProperty()
        {
            var propertyMember = _metadataInfo.FindMember("Namespace1", "TestClass", "TestProperty");
            Assert.IsInstanceOf<Property>(propertyMember);

            var property = (Property) propertyMember;

            Assert.NotNull(property.Getter);
            Assert.NotNull(property.Setter);

            Assert.True(property.CanRead);
            Assert.True(property.CanWrite);

            Assert.AreEqual("Int32", property.ReturnType.Name);
        }


        [TestCase("Namespace1", "TestClass", "ConstProtectedField", Field.Constraint.Const)]
        [TestCase("Namespace1", "TestClass", "ProtectedInternalField", Field.Constraint.None)]
        [TestCase("Namespace1", "TestClass", "ReadOnlyInternalField", Field.Constraint.ReadOnly)]
        public void Analyze_CheckFieldConstraints(
            string nSpace,
            string typeName,
            string memberName,
            Field.Constraint constraint
        )
        {
            var fieldMember = _metadataInfo.FindMember(nSpace, typeName, memberName);

            Assert.IsInstanceOf<Field>(fieldMember);

            var field = (Field) fieldMember;

            Assert.AreEqual(constraint, field.FieldConstraint);
        }

        [TestCase("Namespace1", "TestClass", "ConstProtectedField", "String")]
        [TestCase("Namespace1", "TestClass", "ProtectedInternalField", "Object")]
        [TestCase("Namespace1", "TestClass", "ReadOnlyInternalField", "Int32")]
        public void Analyze_CheckFieldReturnType(
            string nSpace,
            string typeName,
            string memberName,
            string returnType
        )
        {
            var fieldMember = _metadataInfo.FindMember(nSpace, typeName, memberName);

            Assert.IsInstanceOf<Field>(fieldMember);

            var field = (Field) fieldMember;

            Assert.AreEqual(returnType, field.ReturnType.Name);
        }

        [Test]
        public void Analyze_CheckEvent()
        {
            var eventMember = _metadataInfo.FindMember("Namespace1", "TestClass", "TestEvent");

            Assert.IsInstanceOf<Event>(eventMember);

            var eve = (Event) eventMember;

            Assert.NotNull(eve.AddMethod);
            Assert.NotNull(eve.RemoveMethod);
            Assert.Null(eve.RaiseMethod);

            Assert.AreEqual("Action", eve.EventType.Name);
        }


        [TestCase("Namespace1", "TestClass", "PublicVirtualMethod", true, false)]
        [TestCase("Namespace1", "TestClass", "PrivateSealedMethod", false, true)]
        public void Analyze_CheckMethodIsVirtualSealed(
            string nSpace,
            string typeName,
            string memberName,
            bool isVirtual,
            bool isSealed
        )
        {
            var methodMember = _metadataInfo.FindMember(nSpace, typeName, memberName);

            Assert.IsInstanceOf<Method>(methodMember);

            var method = (Method) methodMember;

            Assert.AreEqual(isSealed, method.IsSealed);
            Assert.AreEqual(isVirtual, method.IsVirtual);
        }

        [Test]
        public void Analyze_CheckMethodParametersAndReturnType()
        {
            var methodMember = _metadataInfo.FindMember("Namespace1", "TestClass", "PublicVirtualMethod");

            Assert.IsInstanceOf<Method>(methodMember);

            var method = (Method) methodMember;

            Assert.AreEqual("String", method.ReturnType.Name);

            var indexParam = method.Parameters[0];

            Assert.AreEqual("index", indexParam.Name);
            Assert.AreEqual("Int32", indexParam.ParameterType.Name);

            var strParam = method.Parameters[1];

            Assert.AreEqual("str", strParam.Name);
            Assert.AreEqual("String", strParam.ParameterType.Name);
        }


        [TestCase("Namespace1", "Interface", "PublicNestedType", Access.Public)]
        [TestCase("Namespace1", "Interface", "ProtectedNestedType", Access.Protected)]
        [TestCase("Namespace1", "Interface", "InternalNestedType", Access.Internal)]
        [TestCase("Namespace1", "Interface", "ProtectedInternalNestedType", Access.InternalProtected)]
        [TestCase("Namespace1", "Interface", "PrivateNestedType", Access.Private)]
        public void Analyze_CheckNestedTypeAccess(string nSpace, string typeName, string nestedName, Access access)
        {
            var member = _metadataInfo.FindMember(nSpace, typeName, nestedName);

            Assert.IsInstanceOf<NestedType>(member);

            var nestedType = (NestedType) member;

            Assert.AreEqual(access, nestedType.AccessLevel);
            Assert.AreEqual(access, nestedType.Type.Access);
        }

        [TestCase("Namespace1", "Interface", "PublicNestedType")]
        [TestCase("Namespace1", "Interface", "ProtectedNestedType")]
        [TestCase("Namespace1", "Interface", "InternalNestedType")]
        [TestCase("Namespace1", "Interface", "ProtectedInternalNestedType")]
        [TestCase("Namespace1", "Interface", "PrivateNestedType")]
        public void Analyze_CheckNestedTypeName(string nSpace, string typeName, string nestedName)
        {
            var member = _metadataInfo.FindMember(nSpace, typeName, nestedName);

            Assert.IsInstanceOf<NestedType>(member);

            var nestedType = (NestedType) member;

            Assert.AreEqual(nestedName, nestedType.Type.Name);
        }
    }

    internal static class Extensions
    {
        internal static Namespace FindNamespace(this MetadataInfo metadataInfo, string nSpace)
        {
            return metadataInfo
                .Namespaces
                .Find(n => n.Name == nSpace);
        }

        internal static Type FindType(this MetadataInfo metadataInfo, string nSpace, string type)
        {
            return metadataInfo.FindNamespace(nSpace).Types.Find(t => t.Name == type);
        }

        internal static Member FindMember(this MetadataInfo metadataInfo, string nSpace, string type, string member)
        {
            return metadataInfo.FindType(nSpace, type).Members.Find(m => m.Name == member);
        }
    }
}