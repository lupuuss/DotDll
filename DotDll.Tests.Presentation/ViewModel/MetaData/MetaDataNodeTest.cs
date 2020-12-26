using System.Collections.Generic;
using System.Linq;
using DotDll.Logic.MetaData.Data;
using DotDll.Presentation.ViewModel.MetaData;
using NUnit.Framework;

namespace DotDll.Tests.Presentation.ViewModel.MetaData
{
    [TestFixture]
    public class MetaDataNodeTest
    {
        [SetUp]
        public void SetUp()
        {
            var stringType = new Type("class String");

            var firstNameMember = new Member(
                "(property) public String FirstName",
                new List<Type> {stringType}
            );

            var lastNameMember = new Member(
                "(property) public String LastName",
                new List<Type> {stringType}
            );

            var relatedPersonField = new Member(
                "(field) private Person _relatedPerson",
                new List<Type>());

            var personType = new Type(
                "public class Person",
                new List<Member> {firstNameMember, lastNameMember, relatedPersonField}
            );

            relatedPersonField.RelatedTypes.Add(personType);

            var namespaceObject = new Namespace("Project", new List<Type> {personType});

            _metaData = new MetaDataObject(
                "Project.dll",
                new List<Namespace> {namespaceObject}
            );
        }

        private MetaDataObject _metaData;

        [Test]
        public void Constructor_EveryTypeOfDefinition_LoadsZeroNodes()
        {
            var nodes = new List<MetaDataNode>
            {
                new MetaDataNode(_metaData.Namespaces[0]),
                new MetaDataNode(_metaData.Namespaces[0].Types[0]),
                new MetaDataNode(_metaData.Namespaces[0].Types[0].Members[0])
            };

            foreach (var node in nodes) Assert.AreEqual(0, node.Nodes.Count);
        }

        [Test]
        public void LoadChildren_NamespaceDefinition_LoadsSubNodes()
        {
            var nSpace = _metaData.Namespaces[0];

            var node = new MetaDataNode(nSpace);
            var expectedSize = nSpace.Types.Count;

            node.LoadChildren();

            Assert.AreEqual(expectedSize, node.Nodes.Count);

            var typesNames = nSpace.Types.Select(type => type.Definition);
            var nodesNames = node.Nodes.Select(n => n.Name);

            CollectionAssert.AreEqual(typesNames, nodesNames);
        }

        [Test]
        public void LoadChildren_TypeDefinition_LoadsSubNodes()
        {
            var type = _metaData.Namespaces[0].Types[0];

            var node = new MetaDataNode(type);
            var expectedSize = type.Members.Count;

            node.LoadChildren();

            Assert.AreEqual(expectedSize, node.Nodes.Count);

            var membersNames = type.Members.Select(member => member.Definition);
            var nodesNames = node.Nodes.Select(n => n.Name);

            CollectionAssert.AreEqual(membersNames, nodesNames);
        }

        [Test]
        public void LoadChildren_MemberDefinition_LoadsSubNodes()
        {
            var member = _metaData.Namespaces[0].Types[0].Members[0];

            var node = new MetaDataNode(member);
            var expectedSize = member.RelatedTypes.Count;

            node.LoadChildren();

            Assert.AreEqual(expectedSize, node.Nodes.Count);

            var typesNames = member.RelatedTypes.Select(type => type.Definition);
            var nodesNames = node.Nodes.Select(n => n.Name);

            CollectionAssert.AreEqual(typesNames, nodesNames);
        }

        [Test]
        public void ClearChildren_Always_ClearsSubNodes()
        {
            var node = new MetaDataNode(_metaData.Namespaces[0]);

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
            var node = new MetaDataNode(_metaData.Namespaces[0]);
            var listenerTriggered = false;

            node.IsExpanded = !isExpandedValue;

            node.PropertyChanged += (sender, args) =>
            {
                Assert.IsInstanceOf<MetaDataNode>(sender);
                Assert.AreEqual("IsExpanded", args.PropertyName);

                Assert.AreEqual(isExpandedValue, ((MetaDataNode) sender).IsExpanded);
                listenerTriggered = true;
            };

            node.IsExpanded = isExpandedValue;

            Assert.True(listenerTriggered);
        }

        [Test]
        public void IsExpanded_SetTrue_LoadsChildrenSubNodes()
        {
            var node = new MetaDataNode(_metaData.Namespaces[0]);

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
            var node = new MetaDataNode(_metaData.Namespaces[0]);

            node.LoadChildren();
            node.IsExpanded = true;

            Assert.True(node.Nodes.All(n => n.Nodes.Any()));

            node.IsExpanded = false;

            Assert.True(node.Nodes.All(n => !n.Nodes.Any()));
        }
    }
}