using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DotDll.Logic.Metadata.Data;
using DotDll.Logic.Metadata.Sources;

#if DEBUG
namespace DotDll.Logic.Metadata
{
    public class TempMetadataService : IMetadataService
    {
        public bool IsValidFileSourcePath(string path)
        {
            return File.Exists(path);
        }

        public Source CreateFileSource(string path)
        {
            return new FileSource(path);
        }

        public Task<List<Source>> GetSerializedSources()
        {
            return Task.Run(delegate
            {
                Thread.Sleep(1000);
                return new List<Source>
                {
                    new SerializedSource("Example1"),
                    new SerializedSource("Example2"),
                    new SerializedSource("Example3"),
                    new SerializedSource("Example4"),
                    new SerializedSource("Example5"),
                    new SerializedSource("Example6"),
                    new SerializedSource("Example7"),
                    new SerializedSource("Example8"),
                    new SerializedSource("Example9")
                };
            });
        }

        public Task<MetadataDeclarations> LoadMetaData(Source source)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(1500);

                var stringType = new DType("class String");

                var firstNameMember = new DMember(
                    "(property) public String FirstName",
                    new List<DType> {stringType}
                );

                var lastNameMember = new DMember(
                    "(property) public String LastName",
                    new List<DType> {stringType}
                );

                var relatedPersonField = new DMember(
                    "(field) private Person _relatedPerson",
                    new List<DType>());

                var personType = new DType(
                    "public class Person",
                    new List<DMember> {firstNameMember, lastNameMember, relatedPersonField}
                );

                relatedPersonField.RelatedTypes.Add(personType);

                var namespaceObject = new DNamespace("Project", new List<DType> {personType});

                return new MetadataDeclarations(
                    "Project.dll",
                    new List<DNamespace> {namespaceObject}
                );
            });
        }

        public Task<bool> SaveMetaData(Source source)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(1500);
                return new Random().Next() % 2 == 0;
            });
        }
    }
}
#endif