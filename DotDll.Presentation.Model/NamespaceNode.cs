using DotDll.Model.Data;

namespace DotDll.Presentation.Model
{
    public class NamespaceNode : MetadataNode
    {
        private readonly Namespace _ns;

        public NamespaceNode(Namespace ns)
        {
            _ns = ns;
            Name = _ns.Name;
        }
        
        public override string Name { get; }
        public override void LoadChildren()
        {
            
            foreach (var type in _ns.Types)
            {
                Nodes.Add(new TypeNode(type));
            }
        }
    }
}