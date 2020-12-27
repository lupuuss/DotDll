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
            bool isAbstract
        ) : base(name, accessLevel, isStatic, isAbstract)
        {
            ReturnType = returnType;
        }

        public bool IsVirtual { get; private set; }
        
        public bool IsSealed { get; private set; }
        
        public Type ReturnType { get; }

        public List<Parameter> Parameters { get; protected set; } = new List<Parameter>();

        public List<Type> GenericArguments { get; private set; } = new List<Type>();

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
        
        public class Builder
        {
            private readonly string _name;
            private readonly Access _accessLevel;
            private bool _isVirtual;
            private bool _isSealed;
            private bool _isStatic;
            private bool _isAbstract;
            private Type _returnType = null!;
            private List<Parameter> _parameters = new List<Parameter>();
            private List<Type> _genericArguments = new List<Type>();
            
            public Builder(string name, Access accessLevel)
            {
                _name = name;
                _accessLevel = accessLevel;
            }

            public Builder WithVirtual(bool isVirtual)
            {
                _isVirtual = isVirtual;
                return this;
            }
        
            public Builder WithSealed(bool isSealed)
            {
                _isSealed = isSealed;
                return this;
            }
        
            public Builder WithAbstract(bool isAbstract)
            {
                _isAbstract = isAbstract;
                return this;
            }
        
            public Builder WithStatic(bool isStatic)
            {
                _isStatic = isStatic;
                return this;
            }

            public Builder WithReturnType(Type type)
            {
                _returnType = type;
                return this;
            }

            public Builder WithParameters(List<Parameter> parameters)
            {
                _parameters = parameters;
                return this;
            }

            public Builder WithGenericArguments(List<Type> genericArguments)
            {
                _genericArguments = genericArguments;
                return this;
            }
            
            public Method Build()
            {
                var method = new Method(_name, _accessLevel, _returnType, _isStatic, _isAbstract);

                method.Parameters = _parameters;
                method.GenericArguments = _genericArguments;
                method.IsVirtual = _isVirtual;
                method.IsSealed = _isSealed;
                
                return method;
            }
        }
    }

}