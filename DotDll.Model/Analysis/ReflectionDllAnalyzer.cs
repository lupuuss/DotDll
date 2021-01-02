using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotDll.Model.Data;
using DotDll.Model.Data.Base;
using DotDll.Model.Data.Members;

// ReSharper disable MemberCanBeMadeStatic.Local

namespace DotDll.Model.Analysis
{
    public class ReflectionDllAnalyzer : IDllAnalyzer
    {
        public delegate Assembly AssemblyProvider(string path);

        private readonly BindingFlags _flags;

        private readonly AssemblyProvider _provider;
        private readonly Dictionary<System.Type, Type> _typesMapping = new Dictionary<System.Type, Type>();

        public ReflectionDllAnalyzer(AssemblyProvider provider, bool accessNonPublic = false)
        {
            _provider = provider;

            if (accessNonPublic)
            {
                _flags = BindingFlags.Public | BindingFlags.Instance |
                        BindingFlags.Static | BindingFlags.NonPublic;
                return;
            }
           
            _flags = BindingFlags.Public | BindingFlags.Instance |
                     BindingFlags.Static;
        }

        public MetadataInfo Analyze(string path)
        {
            Assembly assembly = _provider(path);

            _typesMapping.Clear();

            var metadataInfo = new MetadataInfo(assembly.GetName().Name);

            var namespaces = assembly
                .GetTypes()
                .GroupBy(type => type.Namespace);


            foreach (var ns in namespaces)
            {
                var analyzedNamespace = AnalyzeNamespace(ns.Key, ns.ToList());

                metadataInfo.AddNamespace(analyzedNamespace);
            }

            return metadataInfo;
        }

        private Namespace AnalyzeNamespace(string namespaceName, IEnumerable<System.Type> namespaceTypes)
        {
            var nSpace = new Namespace(namespaceName);

            foreach (var systemType in namespaceTypes)
            {
                var type = AnalyzeType(systemType);
                nSpace.AddType(type);
            }

            return nSpace;
        }

        private Type AnalyzeType(System.Type systemType)
        {
            if (_typesMapping.ContainsKey(systemType)) return _typesMapping[systemType];

            var type = new Type(
                systemType.Name,
                AnalyzeTypeAccessLevel(systemType),
                AnalyzeTypeKind(systemType),
                systemType.IsSealed,
                systemType.IsAbstract
            );
            
            type.Attributes.AddRange(
                AnalyzeAttributes(System.Attribute.GetCustomAttributes(systemType, false))
                );

            _typesMapping[systemType] = type;

            var baseTypes = new List<Type>();

            if (systemType.BaseType != null && systemType.BaseType != typeof(object))
            {
                baseTypes.Add(AnalyzeType(systemType.BaseType));   
            }

            baseTypes.AddRange(systemType.GetInterfaces().Select(AnalyzeType));
            
            type.BaseTypes.AddRange(baseTypes);
            
            var genericArgs = systemType.GetGenericArguments().Select(AnalyzeType);

            type.GenericArguments.AddRange(genericArgs);

            if (systemType.IsGenericParameter)
            {
                var genericConstraints = systemType.GetGenericParameterConstraints().Select(AnalyzeType);
                type.GenericConstraints.AddRange(genericConstraints);
            }

            AnalyzeTypeMembers(type, systemType);

            return type;
        }
        
        private void AnalyzeTypeMembers(Type type, System.Type systemType)
        {
            AnalyzeTypeConstructor(type, systemType);
            AnalyzeTypeField(type, systemType);
            AnalyzeTypeProperties(type, systemType);
            AnalyzeTypeEvents(type, systemType);
            AnalyzeTypeMethods(type, systemType);
            AnalyzeNestedType(type, systemType);
        }

        private void AnalyzeTypeConstructor(Type type, System.Type systemType)
        {
            foreach (var constructorInfo in systemType.GetConstructors(_flags))
            {
                var constr = new Constructor(
                    type,
                    AnalyzeAccessLevel(constructorInfo),
                    AnalyzeMethodParameters(constructorInfo.GetParameters())
                );
                
                AnalyzeMemberAttributes(constr, constructorInfo);
                
                type.AddMember(constr);
            }
        }

        private void AnalyzeTypeField(Type type, IReflect systemType)
        {
            foreach (var fieldInfo in systemType.GetFields(_flags))
            {
                Field.Constraint constraint;

                if (fieldInfo.IsLiteral)
                    constraint = Field.Constraint.Const;
                else if (fieldInfo.IsInitOnly)
                    constraint = Field.Constraint.ReadOnly;
                else
                    constraint = Field.Constraint.None;

                var field = new Field(
                    fieldInfo.Name,
                    AnalyzeAccessLevel(fieldInfo),
                    AnalyzeType(fieldInfo.FieldType),
                    fieldInfo.IsStatic,
                    constraint
                );
                
                AnalyzeMemberAttributes(field, fieldInfo);
                type.AddMember(field);
            }
        }

        private void AnalyzeTypeEvents(Type type, System.Type systemType)
        {
            foreach (var eventInfo in systemType.GetEvents(_flags))
            {
                var eve = new Event(
                    eventInfo.Name,
                    AnalyzeMethod(eventInfo.RemoveMethod),
                    AnalyzeMethod(eventInfo.AddMethod),
                    AnalyzeMethod(eventInfo.RaiseMethod)
                );

                AnalyzeMemberAttributes(eve, eventInfo);
                type.AddMember(eve);
            }
        }
        
        private void AnalyzeNestedType(Type type, System.Type systemType)
        {
            foreach (var nested in systemType.GetNestedTypes(_flags))
            {
                var n = new NestedType(AnalyzeType(nested));
                
                AnalyzeMemberAttributes(n, nested);
                
                type.AddMember(n);
            }
        }
        
        private void AnalyzeTypeProperties(Type type, IReflect systemType)
        {
            foreach (var propertyInfo in systemType.GetProperties(_flags))
            {
                var getter = propertyInfo.GetMethod;
                var setter = propertyInfo.SetMethod;

                var property = new Property(propertyInfo.Name, AnalyzeMethod(getter), AnalyzeMethod(setter));
                
                AnalyzeMemberAttributes(property, propertyInfo);
                type.AddMember(property);
            }
        }

        private void AnalyzeTypeMethods(Type type, IReflect systemType)
        {
            foreach (var methodInfo in systemType.GetMethods(_flags))
            {
                var method = AnalyzeMethod(methodInfo);


                if (method != null)
                {
                    type.AddMember(method);
                }
            }
        }
        
        private Method? AnalyzeMethod(MethodInfo? methodInfo)
        {
            if (methodInfo == null) return null;

            var methodBuilder = new Method.Builder(methodInfo.Name, AnalyzeAccessLevel(methodInfo));

            var method = methodBuilder
                .WithAbstract(methodInfo.IsAbstract)
                .WithSealed(methodInfo.IsFinal)
                .WithStatic(methodInfo.IsStatic)
                .WithVirtual(methodInfo.IsVirtual)
                .WithReturnType(AnalyzeType(methodInfo.ReturnType))
                .WithParameters(AnalyzeMethodParameters(methodInfo.GetParameters()))
                .WithGenericArguments(methodInfo.GetGenericArguments().Select(AnalyzeType).ToList())
                .Build();
            
                            
            AnalyzeMemberAttributes(method, methodInfo);

            return method;
        }

        private List<Parameter> AnalyzeMethodParameters(IEnumerable<ParameterInfo> parameters)
        {
            return parameters
                .Select(param =>
                {
                    var p = new Parameter(param.Name, AnalyzeType(param.ParameterType));
                    
                    var attribs = System.Attribute.GetCustomAttributes(param, false);

                    if (attribs != null)
                    {
                        p.Attributes.AddRange(AnalyzeAttributes(attribs));
                    }

                    return p;
                })
                .ToList();
        }
        
        private IEnumerable<Attribute> AnalyzeAttributes(IEnumerable<System.Attribute> attributes)
        {
            
            return attributes
                .Select(a =>
                {
                    var values = a
                        .GetType()
                        .GetProperties()
                        .ToDictionary(p => p.Name, p => p.GetValue(a)?.ToString() ?? "null");
                    
                    return new Attribute(a.GetType().Name.Replace("Attribute", ""), values);
                });
        }
        
        private void AnalyzeMemberAttributes(Member member, MemberInfo memberInfo)
        {
            var attributes = System.Attribute.GetCustomAttributes(memberInfo, false);
            
            member.Attributes.AddRange(AnalyzeAttributes(attributes));
        }
        
        private Type.Kind AnalyzeTypeKind(System.Type type)
        {
            if (type.IsArray) return Type.Kind.Array;

            if (type.IsEnum) return Type.Kind.Enum;

            if (type.IsInterface) return Type.Kind.Interface;

            return type.IsGenericParameter ? Type.Kind.GenericArg : Type.Kind.Class;
        }

        private Access AnalyzeTypeAccessLevel(System.Type type)
        {
            if (type.IsPublic || type.IsNestedPublic) return Access.Public;

            if (type.IsNestedFamANDAssem) return Access.InternalProtected;

            if (type.IsNestedFamily) return Access.Protected;

            return type.IsNestedAssembly ? Access.Internal : Access.Private;
        }
       
        private Access AnalyzeAccessLevel(dynamic info)
        {
            if (info.IsPublic) return Access.Public;

            if (info.IsFamilyAndAssembly) return Access.InternalProtected;

            if (info.IsFamily) return Access.Protected;

            return info.IsAssembly ? Access.Internal : Access.Private;
        }

    }
}