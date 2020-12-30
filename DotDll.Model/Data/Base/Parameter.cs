// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace DotDll.Model.Data.Base
{
    public class Parameter
    {
        public Parameter(string name, Type parameterType)
        {
            Name = name;
            ParameterType = parameterType;
        }

        private Parameter()
        {
            Name = null!;
            ParameterType = null!;
        }

        public string Name { get; private set; }

        public Type ParameterType { get; private set; }
    }
}