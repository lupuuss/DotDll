using System.Collections.Generic;
using DotDll.Model.Analysis.Data.Base;
using DotDll.Model.Analysis.Data.Members;

namespace DotDll.Model.Analysis.Data
{
    public class Type
    {
        public enum Kind
        {
            Interface, Class, Enum, Array, GenericArg
        }
        
        public string Name { get; }
        public Access Access { get;  }
        public Kind TypeKind { get; }
        public bool IsSealed { get; }
        
        public bool IsAbstract { get; }
        
        public bool IsStatic { get; }
        
        public List<Member> Members { get; }

        public List<Type> GenericArguments { get; }
        
        internal Type(
            string name, 
            Access access,
            Kind typeKind,
            bool isSealed,
            bool isAbstract,
            List<Member> members,
            List<Type> genericArguments
            )
        {
            Name = name;
            Access = access;
            TypeKind = typeKind;
            IsSealed = isSealed;
            IsAbstract = isAbstract;
            IsStatic = isSealed && isAbstract;
            Members = members;
            GenericArguments = genericArguments;
        }

        internal Type(
            string name, 
            Access access, 
            Kind typeKind,
            bool isSealed,
            bool isAbstract,
            List<Type> genericArguments
            ) : this(name, access, typeKind, isSealed, isAbstract, new List<Member>(), new List<Type>())
        {
            
        }

        internal void AddMember(Member member)
        {
            Members.Add(member);
        }

        internal void AddGenericArgument(Type genericArgument)
        {
            GenericArguments.Add(genericArgument);
        }
    }
}