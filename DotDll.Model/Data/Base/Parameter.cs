namespace DotDll.Model.Data.Base
{
    public class Parameter
    {
        public Parameter(string name, Type parameterType)
        {
            Name = name;
            ParameterType = parameterType;
        }

        public string Name { get; }

        public Type ParameterType { get; }
    }
}