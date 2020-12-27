using System.Collections.Generic;
using System.Linq;
using DotDll.Model.Data.Base;

namespace DotDll.Model.Data.Members
{
    public class Method : Member
    {
        protected Method(
            string name,
            Access accessLevel,
            Type returnType,
            bool isStatic,
            bool isAbstract,
            List<Parameter> parameters,
            List<Type> genericArguments
        ) : base(name, accessLevel, isStatic, isAbstract)
        {
            ReturnType = returnType;
            Parameters = parameters;
            GenericArguments = genericArguments;
        }

        public Method(
            string name,
            Access accessLevel,
            Type returnType,
            bool isStatic,
            bool isAbstract
        ) : this(name, accessLevel, returnType, isStatic, isAbstract, new List<Parameter>(), new List<Type>())
        {
        }

        public Type ReturnType { get; }

        public List<Parameter> Parameters { get; }

        public List<Type> GenericArguments { get; }

        internal void AddParameter(Parameter parameter)
        {
            Parameters.Add(parameter);
        }

        internal void AddGenericArgument(Type type)
        {
            GenericArguments.Add(type);
        }

        public override IEnumerable<Type> GetRelatedTypes()
        {
            var list = Parameters
                .Select(param => param.ParameterType)
                .ToList();

            return new List<Type>(list)
            {
                ReturnType
            };
        }
    }
}