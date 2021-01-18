using System.Collections.Generic;
using System.Linq;
using DotDll.Model.Data;
using DotDll.Model.Data.Base;
using DotDll.Model.Data.Members;
using DotDll.Presentation.Model;
using NUnit.Framework;

namespace DotDll.Tests.Presentation.ViewModel.MetaData
{
    [TestFixture]
    public class MetaDataNodeTest
    {
        [SetUp]
        public void SetUp()
        {
            var stringType = new Type("String", Access.Private, Type.Kind.Class, false, false);

            var firstNameMember = new Field("firstName", Access.Private, stringType, false);

            var lastNameMember = new Field("firstName", Access.Private, stringType, false);
            
            var personType = new Type(
                "Person",
                Access.Public,
                Type.Kind.Class,
                false,
                false
                );

            var relatedPersonField = new Field("_relatedPerson", Access.Private, personType, false);
            
            personType.Members.AddRange(new List<Field> {firstNameMember, lastNameMember, relatedPersonField});

            var namespaceObject = new Namespace("Project", new List<Type> {personType});

            _metadata = new MetadataInfo(
                "Project.dll",
                new List<Namespace> {namespaceObject}
            );
        }

        private MetadataInfo _metadata;

        [Test]
        public void Constructor_EveryTypeOfDefinition_LoadsZeroNodes()
        {
            var nodes = new List<MetadataNode>
            {
                new NamespaceNode(_metadata.Namespaces[0]),
                new TypeNode(_metadata.Namespaces[0].Types[0]),
                new FieldNode((Field) _metadata.Namespaces[0].Types[0].Members[0])
            };

            foreach (var node in nodes) Assert.AreEqual(0, node.Nodes.Count);
        }

        [Test]
        public void LoadChildren_NamespaceDefinition_LoadsSubNodes()
        {
            var nSpace = _metadata.Namespaces[0];

            var node = new NamespaceNode(nSpace);
            var expectedSize = nSpace.Types.Count;

            node.LoadChildren();

            Assert.AreEqual(expectedSize, node.Nodes.Count);
        }

        [Test]
        public void LoadChildren_TypeDefinition_LoadsSubNodes()
        {
            var type = _metadata.Namespaces[0].Types[0];

            var node = new TypeNode(type);
            var expectedSize = type.Members.Count;

            node.LoadChildren();

            Assert.AreEqual(expectedSize, node.Nodes.Count);
        }

        [Test]
        public void LoadChildren_MemberDefinition_LoadsSubNodes()
        {
            var member = _metadata.Namespaces[0].Types[0].Members[0];

            var node = new FieldNode((Field) member);

            node.LoadChildren();

            Assert.AreEqual(1, node.Nodes.Count);
        }

        [Test]
        public void ClearChildren_Always_ClearsSubNodes()
        {
            var node = new NamespaceNode(_metadata.Namespaces[0]);

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
            var node = new NamespaceNode(_metadata.Namespaces[0]);
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
            var node = new NamespaceNode(_metadata.Namespaces[0]);

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
            var node = new NamespaceNode(_metadata.Namespaces[0]);

            node.LoadChildren();
            node.IsExpanded = true;

            Assert.True(node.Nodes.All(n => n.Nodes.Any()));

            node.IsExpanded = false;

            Assert.True(node.Nodes.All(n => !n.Nodes.Any()));
        }
    }
}