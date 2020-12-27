using System.Collections.Generic;
using System.Linq;
using DotDll.Logic.Metadata.Data;
using DotDll.Presentation.ViewModel.Metadata;
using NUnit.Framework;

namespace DotDll.Tests.Presentation.ViewModel.MetaData
{
    [TestFixture]
    public class MetaDataNodeTest
    {
        [SetUp]
        public void SetUp()
        {
            var stringType = new DType("class String");

            var firstNameMember = new DMember(
                "(property) public String FirstName",
                new List<DType> {stringType}
            );

            var lastNameMember = new DMember(
                "(property) public String LastName",
                new List<DType> {stringType}
            );

            var relatedPersonField = new DMember(
                "(field) private Person _relatedPerson",
                new List<DType>());

            var personType = new DType(
                "public class Person",
                new List<DMember> {firstNameMember, lastNameMember, relatedPersonField}
            );

            relatedPersonField.RelatedTypes.Add(personType);

            var namespaceObject = new DNamespace("Project", new List<DType> {personType});

            _metaData = new MetaDataDeclarations(
                "Project.dll",
                new List<DNamespace> {namespaceObject}
            );
        }

        private MetaDataDeclarations _metaData;

        [Test]
        public void Constructor_EveryTypeOfDefinition_LoadsZeroNodes()
        {
            var nodes = new List<MetadataNode>
            {
                new MetadataNode(_metaData.Namespaces[0]),
                new MetadataNode(_metaData.Namespaces[0].Types[0]),
                new MetadataNode(_metaData.Namespaces[0].Types[0].Members[0])
            };

            foreach (var node in nodes) Assert.AreEqual(0, node.Nodes.Count);
        }

        [Test]
        public void LoadChildren_NamespaceDefinition_LoadsSubNodes()
        {
            var nSpace = _metaData.Namespaces[0];

            var node = new MetadataNode(nSpace);
            var expectedSize = nSpace.Types.Count;

            node.LoadChildren();

            Assert.AreEqual(expectedSize, node.Nodes.Count);

            var typesNames = nSpace.Types.Select(type => type.Declaration);
            var nodesNames = node.Nodes.Select(n => n.Name);

            CollectionAssert.AreEqual(typesNames, nodesNames);
        }

        [Test]
        public void LoadChildren_TypeDefinition_LoadsSubNodes()
        {
            var type = _metaData.Namespaces[0].Types[0];

            var node = new MetadataNode(type);
            var expectedSize = type.Members.Count;

            node.LoadChildren();

            Assert.AreEqual(expectedSize, node.Nodes.Count);

            var membersNames = type.Members.Select(member => member.Declaration);
            var nodesNames = node.Nodes.Select(n => n.Name);

            CollectionAssert.AreEqual(membersNames, nodesNames);
        }

        [Test]
        public void LoadChildren_MemberDefinition_LoadsSubNodes()
        {
            var member = _metaData.Namespaces[0].Types[0].Members[0];

            var node = new MetadataNode(member);
            var expectedSize = member.RelatedTypes.Count;

            node.LoadChildren();

            Assert.AreEqual(expectedSize, node.Nodes.Count);

            var typesNames = member.RelatedTypes.Select(type => type.Declaration);
            var nodesNames = node.Nodes.Select(n => n.Name);

            CollectionAssert.AreEqual(typesNames, nodesNames);
        }

        [Test]
        public void ClearChildren_Always_ClearsSubNodes()
        {
            var node = new MetadataNode(_metaData.Namespaces[0]);

            node.ClearChildren();

            Assert.IsEmpty(node.Nodes);

            node.LoadChildren();

            Assert.IsNotEmpty(node.Nodes);

            node.ClearChildren();

            Assert.IsEmpty(node.Nodes);

            node.ClearChildren();

            Assert.IsEmpty(node.Nodes);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void IsExpanded_ListenerRegistered_NotifyAboutChanges(bool isExpandedValue)
        {
            var node = new MetadataNode(_metaData.Namespaces[0]);
            var listenerTriggered = false;

            node.IsExpanded = !isExpandedValue;

            node.PropertyChanged += (sender, args) =>
            {
                Assert.IsInstanceOf<MetadataNode>(sender);
                Assert.AreEqual("IsExpanded", args.PropertyName);

                Assert.AreEqual(isExpandedValue, ((MetadataNode) sender).IsExpanded);
                listenerTriggered = true;
            };

            node.IsExpanded = isExpandedValue;

            Assert.True(listenerTriggered);
        }

        [Test]
        public void IsExpanded_SetTrue_LoadsChildrenSubNodes()
        {
            var node = new MetadataNode(_metaData.Namespaces[0]);

            node.LoadChildren();

            // initially checks if children are loaded and if they are empty
            Assert.IsNotEmpty(node.Nodes);
            Assert.True(node.Nodes.All(n => !n.Nodes.Any()));

            node.IsExpanded = true;

            Assert.True(node.Nodes.All(n => n.Nodes.Any()));
        }

        [Test]
        public void IsExpanded_SetFalse_ClearsChildrenSubNodes()
        {
            var node = new MetadataNode(_metaData.Namespaces[0]);

            node.LoadChildren();
            node.IsExpanded = true;

            Assert.True(node.Nodes.All(n => n.Nodes.Any()));

            node.IsExpanded = false;

            Assert.True(node.Nodes.All(n => !n.Nodes.Any()));
        }
    }
}