using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotDll.Logic.MetaData;
using DotDll.Logic.MetaData.Data;
using DotDll.Logic.MetaData.Sources;
using DotDll.Model.Analysis;
using DotDll.Model.Data;
using DotDll.Model.Data.Base;
using DotDll.Model.Data.Members;
using DotDll.Model.Files;
using DotDll.Model.Serialization;
using Member = DotDll.Logic.MetaData.Data.Member;
using Namespace = DotDll.Logic.MetaData.Data.Namespace;
using Type = DotDll.Logic.MetaData.Data.Type;

namespace DotDll.Logic
{
    public class InvalidFileException : Exception
    {
        public InvalidFileException() : base("File doesn't exists or has bad extension (must be dll or exe)")
        {
        }
    }
    
    public class MetaDataService : IMetaDataService
    {

        private readonly IFilesManager _filesManager;

        private readonly IDllInfoSerializer _serializer;

        private readonly IDllAnalyzer _analyzer;

        private Dictionary<DllInfo, MetaDataObject> _objectsMapping = new Dictionary<DllInfo, MetaDataObject>();

        public MetaDataService(IFilesManager filesManager, IDllInfoSerializer serializer, IDllAnalyzer analyzer)
        {
            _filesManager = filesManager;
            _serializer = serializer;
            _analyzer = analyzer;
        }

        public bool IsValidFileSourcePath(string path)
        {
            return _filesManager.FileExists(path) &&
                   (_filesManager.GetExtension(path) == "dll" || _filesManager.GetExtension(path) == "exe");
        }

        public Source CreateFileSource(string path)
        {
            if (IsValidFileSourcePath(path))
            {
                return new FileSource(path);
            }

            throw new InvalidFileException();
        }

        public Task<List<Source>> GetSerializedSources()
        {
            return Task.Run(delegate
            { 
                return _serializer
                    .GetAllIds()
                    .Select(id => new SerializedSource(id))
                    .Cast<Source>()
                    .ToList();
            });
        }

        public Task<MetaDataObject> LoadMetaData(Source source)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SaveMetaData(Source source)
        {
            throw new System.NotImplementedException();
        }

        private MetaDataObject Map(DllInfo dllInfo)
        {
            var namespaces = dllInfo.Namespaces
                .Select(MapNamespace)
                .ToList();

            var obj = new MetaDataObject(dllInfo.Name, namespaces);

            _objectsMapping[dllInfo] = obj;

            return obj;
        }

        private Namespace MapNamespace(DotDll.Model.Data.Namespace nSpace)
        {
            var types = nSpace.Types
                .Select(MapType)
                .ToList();

            return new Namespace(nSpace.Name, types);
        }

        private Type MapType(Model.Data.Type type)
        {

            if (type.TypeKind is Model.Data.Type.Kind.GenericArg)
            {
                return new Type(type.Name);
            }
            
            string name = GetAccessString(type.Access);

            if (type.IsStatic)
            {
                name += " static";
            }
            else
            {
                if (type.IsSealed)
                {
                    name += " sealed";
                }
                
                if (type.IsAbstract)
                {
                    name += " abstract";
                }
            }

            name += " ";
            
            switch (type.TypeKind)
            {
                case Model.Data.Type.Kind.Interface:
                    name += "interface";
                    break;
                case Model.Data.Type.Kind.Class:
                    name += "class";
                    break;
                case Model.Data.Type.Kind.Enum:
                    name += "enum";
                    break;
                case Model.Data.Type.Kind.Array:
                    name += "[]";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            name += " " + type.Name;

            if (type.GenericArguments.Any())
            {
                name += "<";
                name += String.Join(", ", type.GenericArguments.Select(arg => arg.Name));
                name += ">";
            }

            return new Type(name, type.Members.Select(MapMember).ToList());
        }

        private Member MapMember(Model.Data.Base.Member member)
        {

            switch (member)
            {
                case Event eve:
                    return MapEvent(eve);
                case Constructor constructor:
                    return MapConstructor(constructor);
                case Field field:
                    return MapField(field);
                case Method method:
                    return MapMethod(method);
                case NestedType nestedType:
                    return MapNestedType(nestedType);
                case Property property:
                    return MapProperty(property);
                default:
                    throw new ArgumentOutOfRangeException(nameof(member));
            }
        }

        private Member MapProperty(Property property)
        {
            throw new NotImplementedException();
        }

        private Member MapNestedType(NestedType nestedType)
        {
            throw new NotImplementedException();
        }

        private Member MapMethod(Method method)
        {
            throw new NotImplementedException();
        }

        private Member MapField(Field field)
        {
            throw new NotImplementedException();
        }

        private Member MapConstructor(Constructor constructor)
        {
            throw new NotImplementedException();
        }

        private Member MapEvent(Event eve)
        {
            throw new NotImplementedException();
        }

        private string GetAccessString(Access typeAccess)
        {
            switch (typeAccess)
            {
                case Access.Public:
                    return "public";
                case Access.Internal:
                    return "public";
                case Access.Protected:
                    return "protected";
                case Access.InternalProtected:
                    return "internal protected";;
                case Access.Private:
                    return "private";
                case Access.Inner:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeAccess), typeAccess, null);
            }
        }

        private DllInfo Map(MetaDataObject metaData)
        {
            return _objectsMapping
                .First(entry => entry.Value == metaData)
                .Key;
        }
    }
}