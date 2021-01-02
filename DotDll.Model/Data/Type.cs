using System.Collections.Generic;
using System.Linq;
using DotDll.Model.Data.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace DotDll.Model.Data
{
    public class Type
    {
        public enum Kind
        {
            Interface,
            Class,
            Enum,
            Array,
            GenericArg
        }

        public Type(
            string name,
            Access access,
            Kind typeKind,
            bool isSealed,
            bool isAbstract
        )
        {
            Name = name;
            Access = access;
            TypeKind = typeKind;
            IsSealed = isSealed;
            IsAbstract = isAbstract;
            IsStatic = isSealed && isAbstract;
        }

        private Type()
        {
            Name = null!;
            Members = null!;
            GenericArguments = null!;
        }

        public string Name { get; private set; }

        public Access Access { get; private set; }

        public Kind TypeKind { get; private set; }

        public bool IsSealed { get; private set; }

        public bool IsAbstract { get; private set; }

        public bool IsStatic { get; private set; }

        public List<Member> Members { get; private set; } = new List<Member>();

        public List<Type> GenericArguments { get; private set; } = new List<Type>();

        public List<Type> GenericConstraints { get; private set; } = new List<Type>();

        public List<Type> BaseTypes { get; private set; } = new List<Type>();
        
        public List<Attribute> Attributes { get; private set; } = new List<Attribute>();

        public string FullName()
        {
            if (GenericArguments.Any())
                return Name + $"<{string.Join(", ", GenericArguments.Select(arg => arg.Name))}>";

            if (GenericConstraints.Any())
                return Name + $" : {string.Join(", ", GenericConstraints.Select(arg => arg.Name))}";

            return Name;
        }

        internal void AddMember(Member member)
        {
            Members.Add(member);
        }
    }
}