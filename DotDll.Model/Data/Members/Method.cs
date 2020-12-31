using System.Collections.Generic;
using System.Linq;
using DotDll.Model.Data.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace DotDll.Model.Data.Members
{
    public class Method : Member
    {
        public Method(
            string name,
            Access accessLevel,
            Type returnType,
            bool isStatic,
            bool isAbstract
        ) : base(name, accessLevel, isStatic, isAbstract)
        {
            ReturnType = returnType;
        }

        protected Method()
        {
            ReturnType = null!;
        }

        public bool IsVirtual { get; private set; }

        public bool IsSealed { get; private set; }

        public Type ReturnType { get; private set; }

        public List<Parameter> Parameters { get; protected set; } = new List<Parameter>();

        public List<Type> GenericArguments { get; private set; } = new List<Type>();

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
            private readonly Access _accessLevel;
            private readonly string _name;
            private List<Type> _genericArguments = new List<Type>();
            private bool _isAbstract;
            private bool _isSealed;
            private bool _isStatic;
            private bool _isVirtual;
            private List<Parameter> _parameters = new List<Parameter>();
            private Type _returnType = null!;

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
                return new Method(_name, _accessLevel, _returnType, _isStatic, _isAbstract)
                {
                    Parameters = _parameters,
                    GenericArguments = _genericArguments,
                    IsVirtual = _isVirtual,
                    IsSealed = _isSealed
                };
            }
        }
    }
}