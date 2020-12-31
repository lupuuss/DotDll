using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotDll.Model.Data;
using DotDll.Model.Data.Base;
using DotDll.Model.Data.Members;
using DotDll.Model.Files;
using DotDll.Model.Serialization;
using DotDll.Model.Serialization.File;
using Moq;
using NUnit.Framework;

namespace DotDll.Tests.Model.Serialization.File
{
    [TestFixture(FileType.Xml)]
    public class FileMetadataSerializerTest
    {
        private readonly FileType _type;

        public FileMetadataSerializerTest(FileType type)
        {
            _type = type;
        }
        
        private IMetadataSerializer _serializer;
        private Mock<IFilesManager> _filesManagerMock;
        private IFilesManager _filesManager;
        private Dictionary<string, MemoryStream> _streams;

        [SetUp]
        public void SetUp()
        {
            _filesManagerMock = new Mock<IFilesManager>();

            _streams = new Dictionary<string, MemoryStream>();
            
            _filesManagerMock
                .Setup(f => f.FileExists(It.IsAny<string>()))
                .Returns<string>(path => _streams.ContainsKey(path));

            _filesManagerMock
                .Setup(f => f.PathExists(It.IsAny<string>()))
                .Returns(true);

            _filesManagerMock
                .Setup(f => f.FileInPath(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>(Path.Combine);
            
            _filesManagerMock
                .Setup(f => f.OpenFileRead(It.IsAny<string>()))
                .Returns<string>((path) => new MemoryStream(_streams[path].ToArray()));
            
            _filesManagerMock
                .Setup(f => f.OpenFileWrite(It.IsAny<string>()))
                .Returns<string>((path) =>
                {
                    _streams[path] = new MemoryStream();
                    return _streams[path];
                });
            _filesManager = _filesManagerMock.Object;
            _serializer = FileMetadataSerializer.Create(".\\files\\", _filesManager, _type);
        }

        [Test]
        public void GetAllIds_Always_ReturnsSerializedIds()
        {
            var metadata1 = new MetadataInfo("Name1");
            var metadata2 = new MetadataInfo("Name2");

            _serializer.Serialize(metadata1);

            var ids = _serializer.GetAllIds();
            
            CollectionAssert.AreEqual(new [] {"Name1_0"}, ids);
            
            _serializer.Serialize(metadata2);
            
            ids = _serializer.GetAllIds();

            CollectionAssert.AreEqual(new [] {"Name1_0", "Name2_0"}, ids);
        }

        [Test]
        public void GetAllIds_Always_ReturnsOnlyExistingFiles()
        {
            var metadata1 = new MetadataInfo("Name1");
            var metadata2 = new MetadataInfo("Name2");

            _serializer.Serialize(metadata1);
            _serializer.Serialize(metadata2);

            var key = _streams.Keys.First(x => x.Contains("Name2_0"));
            _streams.Remove(key);

            var actual = _serializer.GetAllIds();
            CollectionAssert.AreEqual(new []{"Name1_0"}, actual);
        }

        [Test]
        public void Serialize_Always_ProvidesUniqueName()
        {
            var metadata = new MetadataInfo("Name");
            
            _serializer.Serialize(metadata);
            _serializer.Serialize(metadata);
            _serializer.Serialize(metadata);

            var ids = _serializer.GetAllIds();
            
            CollectionAssert.AreEqual(new [] {"Name_0", "Name_1", "Name_2"}, ids);
        }

        [Test]
        public void Serialize_Deserialize_CheckStructureAndCircularReference()
        {
            // SETUP
            
            var metadataInfo = new MetadataInfo("TestMetadataInfo");
            var nSpace = new Namespace("Namespace1");
            
            var type = new Type("Class1", Access.Public, Type.Kind.Class, true, false);

            type.Members.Add(new Field("CircularField", Access.Protected, type, false));
            
            nSpace.AddType(type);
            
            metadataInfo.AddNamespace(nSpace);

            // CALL
            
            _serializer.Serialize(metadataInfo);

            var deserializedMetadataInfo = _serializer.Deserialize("TestMetadataInfo_0");
            
            // ASSERT
            
            Assert.AreEqual("TestMetadataInfo", deserializedMetadataInfo.Name);
            Assert.AreEqual(1, deserializedMetadataInfo.Namespaces.Count);

            var deserializedNs = deserializedMetadataInfo.Namespaces[0];
            
            Assert.AreEqual("Namespace1", deserializedNs.Name);
            Assert.AreEqual(1, deserializedMetadataInfo.Namespaces[0].Types.Count);

            var deserializedType = deserializedNs.Types[0];
            
            Assert.AreEqual("Class1", deserializedType.Name);
            Assert.AreEqual(1, deserializedType.Members.Count);

            var deserializedMember = deserializedType.Members[0];
            
            Assert.IsInstanceOf<Field>(deserializedMember);

            var deserializedField = (Field) deserializedMember;
            
            Assert.AreEqual(deserializedType, deserializedField.ReturnType);
            Assert.AreEqual("CircularField", deserializedField.Name);
            Assert.AreEqual(Access.Protected, deserializedField.AccessLevel);
            Assert.False(deserializedField.IsStatic);

        }
        
        [TestCase("Class1", Access.Public, Type.Kind.Class, true, true)]
        [TestCase("Class2", Access.Protected, Type.Kind.Interface, true, false)]
        [TestCase("Class3", Access.Internal, Type.Kind.Enum, false, true)]
        [TestCase("Class4", Access.InternalProtected, Type.Kind.Array, false, false)]
        [TestCase("Class5", Access.Private, Type.Kind.GenericArg, true, true)]
        public void Serialize_Deserialize_CheckType(
            string typeName,
            Access access, 
            Type.Kind kind, 
            bool isSealed, 
            bool isAbstract
            )
        {
            
            // SETUP
            
            var metadataInfo = new MetadataInfo("TestMetadataInfo");
            var nSpace = new Namespace("Namespace1");
            
            var type = new Type(typeName, access, kind, isSealed, isAbstract);
            var baseType = new Type("BaseType", Access.Public, Type.Kind.Class, false, true);
            var generic = new Type("T", Access.Public, Type.Kind.GenericArg, false, false);
            generic.GenericConstraints.Add(baseType);
            
            baseType.Members.Add(new NestedType(type));

            var field = new Field("Field1", Access.Private, type, false, Field.Constraint.ReadOnly);
            
            type.BaseTypes.Add(baseType);

            type.Members.Add(field);
            type.Members.Add(field);
            type.Members.Add(field);
            type.GenericArguments.Add(generic);

            nSpace.AddType(type);
            nSpace.AddType(baseType);
            metadataInfo.AddNamespace(nSpace);
            
            // CALL
            
            _serializer.Serialize(metadataInfo);

            var deserialized = _serializer.Deserialize("TestMetadataInfo_0");

            var deserializedType = deserialized.Namespaces[0].Types[0];
            
            // ASSERT
            
            Assert.AreEqual(typeName, deserializedType.Name);
            Assert.AreEqual(access, deserializedType.Access);
            Assert.AreEqual(kind, deserializedType.TypeKind);
            Assert.AreEqual(isSealed, deserializedType.IsSealed);
            Assert.AreEqual(isAbstract, deserializedType.IsAbstract);
            Assert.AreEqual(3, deserializedType.Members.Count);
            Assert.AreEqual("BaseType", deserializedType.BaseTypes[0].Name);
            Assert.AreEqual(1, deserializedType.GenericArguments.Count);
            Assert.AreEqual("T", deserializedType.GenericArguments[0].Name);
            Assert.True(deserializedType.GenericArguments[0].TypeKind == Type.Kind.GenericArg);
            Assert.AreEqual("BaseType", deserializedType.GenericArguments[0].GenericConstraints[0].Name);
            Assert.True(deserializedType.Members.All(t => t is Field));
        }

        [TestCase(Access.Public, true, true, true, true)]
        [TestCase(Access.Protected, false, false, false, false)]
        [TestCase(Access.Internal, true, false, false, true)]
        [TestCase(Access.Private, false, true, true, false)]
        public void Serialize_Deserialize_CheckMethod(Access access, bool isSealed, bool isStatic, bool isAbstract, bool isVirtual)
        {
            
            // SETUP
            
            var metadataInfo = new MetadataInfo("TestMetadataInfo");
            var nSpace = new Namespace("Namespace1");
            
            var type = new Type("Class", Access.Public, Type.Kind.Class, false, true);

            var argType = new Type("String", Access.Public, Type.Kind.Class, true, false);
            
            var genericType = new Type("T", Access.Public, Type.Kind.GenericArg, false, false);
            
            var method = new Method.Builder("Method", access)
                .WithReturnType(argType)
                .WithSealed(isSealed)
                .WithStatic(isStatic)
                .WithAbstract(isAbstract)
                .WithVirtual(isVirtual)
                .WithParameters(new List<Parameter>() {new Parameter("str", argType), new Parameter("generic", genericType)})
                .WithGenericArguments(new List<Type>() {genericType})
                .Build();
            
            type.Members.Add(method);
            
            nSpace.AddType(type);
            nSpace.AddType(argType);
            
            metadataInfo.AddNamespace(nSpace);
            
            // CALL
            
            _serializer.Serialize(metadataInfo);

            var deserialized = _serializer.Deserialize("TestMetadataInfo_0");

            var deserializedType = deserialized.Namespaces[0].Types[0];
            var deserializedArgType = deserialized.Namespaces[0].Types[1];

            var deserializedMethod = (Method) deserializedType.Members[0];

            // ASSERT
            
            Assert.AreEqual("Method", deserializedMethod.Name);
            Assert.AreEqual(access, deserializedMethod.AccessLevel);
            Assert.AreEqual(isSealed, deserializedMethod.IsSealed);
            Assert.AreEqual(isVirtual, deserializedMethod.IsVirtual);
            Assert.AreEqual(isStatic, deserializedMethod.IsStatic);
            Assert.AreEqual(isAbstract, deserializedMethod.IsAbstract);
            Assert.AreEqual(deserializedArgType, deserializedMethod.ReturnType);

            var param1 = deserializedMethod.Parameters[0];
            
            Assert.AreEqual("str", param1.Name);
            Assert.AreEqual(deserializedArgType, param1.ParameterType);

            var param2 = deserializedMethod.Parameters[1];

            Assert.AreEqual(1, deserializedMethod.GenericArguments.Count);
            
            var deserializedGenericType = deserializedMethod.GenericArguments[0];
            
            Assert.AreEqual("generic", param2.Name);
            Assert.AreEqual(deserializedGenericType, param2.ParameterType);
            Assert.AreEqual("T", deserializedGenericType.Name);
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public void Serialize_Deserialize_CheckProperty(bool applyGetter, bool applySetter)
        {
            var metadataInfo = new MetadataInfo("TestMetadataInfo");
            var nSpace = new Namespace("Namespace1");

            var propertyType = new Type("string", Access.Public, Type.Kind.Class, true, false);
            var type = new Type("Class", Access.Public, Type.Kind.Class, false, true);

            var getter = applyGetter 
                ? new Method.Builder("Getter", Access.Protected)
                    .WithReturnType(propertyType)
                    .Build() 
                : null;
            
            var setter = applySetter 
                ? new Method.Builder("Setter", Access.Private)
                    .WithParameters(new List<Parameter> {new Parameter("x", propertyType)})
                    .Build() 
                : null;
            
            type.Members.Add(new Property("Property", getter, setter));
            
            nSpace.AddType(type);

            metadataInfo.AddNamespace(nSpace);
            
            // CALL
            
            _serializer.Serialize(metadataInfo);

            var deserialized = _serializer.Deserialize("TestMetadataInfo_0");

            var deserializedType = deserialized.Namespaces[0].Types[0];

            var deserializedProperty = (Property) deserializedType.Members[0];

            // ASSERT
            
            Assert.AreEqual("Property", deserializedProperty.Name);
            Assert.AreEqual("string", deserializedProperty.ReturnType.Name);
            
            if (applyGetter)
            {
                Assert.NotNull(deserializedProperty.Getter);
                Assert.AreEqual("Getter", deserializedProperty.Getter.Name);
                Assert.AreEqual(Access.Protected, deserializedProperty.Getter.AccessLevel);
                Assert.True(deserializedProperty.CanRead);
            }
            else
            {
                Assert.False(deserializedProperty.CanRead);
            }
            
            if (applySetter)
            {
                Assert.NotNull(deserializedProperty.Setter);
                Assert.AreEqual("Setter", deserializedProperty.Setter.Name);
                Assert.AreEqual(Access.Private, deserializedProperty.Setter.AccessLevel);
                Assert.True(deserializedProperty.CanWrite);
            }
            else
            {
                Assert.False(deserializedProperty.CanWrite);
            }

        }

        [TestCase(Field.Constraint.None, true)]
        [TestCase(Field.Constraint.ReadOnly, false)]
        [TestCase(Field.Constraint.Const, true)]
        public void Serialize_Deserialize_CheckField(Field.Constraint constraint, bool isStatic)
        {

            var metadataInfo = new MetadataInfo("TestMetadataInfo");
            var nSpace = new Namespace("Namespace1");

            var fieldType = new Type("string", Access.Public, Type.Kind.Class, true, false);
            var type = new Type("Class", Access.Public, Type.Kind.Class, false, true);

            type.Members.Add(new Field("Field", Access.Protected, fieldType, isStatic, constraint));

            nSpace.AddType(type);

            metadataInfo.AddNamespace(nSpace);
            
            // CALL
            
            _serializer.Serialize(metadataInfo);

            var deserialized = _serializer.Deserialize("TestMetadataInfo_0");

            var deserializedType = deserialized.Namespaces[0].Types[0];
            
            var deserializedField = (Field) deserializedType.Members[0];

            // ASSERT
            
            Assert.AreEqual("Field", deserializedField.Name);
            Assert.AreEqual("string", deserializedField.ReturnType.Name);
            Assert.AreEqual(Access.Protected, deserializedField.AccessLevel);
            
            Assert.AreEqual(isStatic, deserializedField.IsStatic);
            Assert.AreEqual(constraint, deserializedField.FieldConstraint);
        }

        [Test]
        public void Serialize_Deserialize_CheckNestedType()
        {
            var metadataInfo = new MetadataInfo("TestMetadataInfo");
            var nSpace = new Namespace("Namespace1");

            var nestedType = new Type("Nested", Access.Protected, Type.Kind.Class, true, false);
            var type = new Type("Class", Access.Public, Type.Kind.Class, false, true);

            type.Members.Add(new NestedType(nestedType));

            nSpace.AddType(type);

            metadataInfo.AddNamespace(nSpace);
            
            // CALL
            
            _serializer.Serialize(metadataInfo);

            var deserialized = _serializer.Deserialize("TestMetadataInfo_0");

            var deserializedType = deserialized.Namespaces[0].Types[0];
            
            var deserializedNested = (NestedType) deserializedType.Members[0];

            // ASSERT
            
            Assert.AreEqual("Nested", deserializedNested.Name);
            Assert.AreEqual("Nested", deserializedNested.Type.Name);
            Assert.AreEqual(Access.Protected, deserializedNested.AccessLevel);
            Assert.AreEqual(Access.Protected, deserializedNested.Type.Access);
        }
        
        [Test]
        public void Serialize_Deserialize_CheckEvent()
        {
            var metadataInfo = new MetadataInfo("TestMetadataInfo");
            var nSpace = new Namespace("Namespace1");

            var eventType = new Type("Action", Access.Public, Type.Kind.Class, true, false);
            var type = new Type("Class", Access.Public, Type.Kind.Class, false, true);

            var parameters = new List<Parameter> {new Parameter("param", eventType)};

            var addMethod = new Method.Builder("_add", Access.Public).WithParameters(parameters).Build();
            
            var removeMethod = new Method.Builder("_remove", Access.Public).WithParameters(parameters).Build();
            
            type.Members.Add(new Event("Event", removeMethod, addMethod , null));
            
            nSpace.AddType(type);

            metadataInfo.AddNamespace(nSpace);
            
            // CALL
            
            _serializer.Serialize(metadataInfo);

            var deserialized = _serializer.Deserialize("TestMetadataInfo_0");

            var deserializedType = deserialized.Namespaces[0].Types[0];

            var deserializedEvent = (Event) deserializedType.Members[0];

            // ASSERT

            Assert.AreEqual("Event", deserializedEvent.Name);
            
            Assert.NotNull(deserializedEvent.AddMethod);
            Assert.AreEqual("_add", deserializedEvent.AddMethod.Name);
            
            Assert.NotNull(deserializedEvent.RemoveMethod);
            Assert.AreEqual("_remove", deserializedEvent.RemoveMethod.Name);
            
            Assert.Null(deserializedEvent.RaiseMethod);
            
            Assert.AreEqual("Action", deserializedEvent.EventType.Name);
        }
    }
}