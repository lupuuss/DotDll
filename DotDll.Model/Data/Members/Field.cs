using System.Collections.Generic;
using DotDll.Model.Data.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace DotDll.Model.Data.Members
{
    public class Field : Member
    {
        public enum Constraint
        {
            None,
            ReadOnly,
            Const
        }

        public Field(
            string name, Access accessLevel, Type returnType, bool isStatic, Constraint constraint = Constraint.None
        ) : base(name, accessLevel, isStatic, false)
        {
            ReturnType = returnType;
            FieldConstraint = constraint;
        }

        private Field()
        {
            ReturnType = null!;
        }

        public Constraint FieldConstraint { get; private set; }

        public Type ReturnType { get; private set; }

        public override IEnumerable<Type> GetRelatedTypes()
        {
            return new List<Type>
            {
                ReturnType
            };
        }
    }
}