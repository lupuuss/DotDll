using DotDll.Model.Data.Base;

namespace DotDll.Presentation.Model
{
    public abstract class MemberNode<T> : MetadataNode where T : Member
    {
        protected MemberNode(T member)
        {
            Member = member;
        }

        protected T Member { get; }

        public override void LoadChildren()
        {
            foreach (var type in Member.GetRelatedTypes())
            {
                Nodes.Add(new TypeNode(type));
            }
        }
    }
}