using System.Collections.Generic;
using System.Linq;
using DotDll.Model.Data.Base;

namespace DotDll.Model.Data.Members
{
    public class Method : Member
    {
        public Type ReturnType { get; }

        public List<Parameter> Parameters { get; }

        public List<Type> GenericArguments { get; }

        protected Method(
            string name,
            Access accessLevel, 
            Type returnType,
            Kind memberKind,
            bool isStatic,
            bool isAbstract,
            List<Parameter> parameters,
            List<Type> genericArguments
            ) : base(name, accessLevel, memberKind, isStatic, isAbstract)
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
            bool isAbstract,
            List<Parameter> parameters, 
            List<Type> genericArguments
            ) : this(name, accessLevel, returnType, Kind.Method, isStatic, isAbstract, parameters, genericArguments)
        {
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

        internal void AddParameter(Parameter parameter)
        {
            Parameters.Add(parameter);
        }

        public override List<Type> GetRelatedTypes()
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