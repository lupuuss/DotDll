using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DotDll.Logic.MetaData.Data;
using DotDll.Logic.MetaData.Sources;
using Type = DotDll.Logic.MetaData.Data.Type;

#if DEBUG
namespace DotDll.Logic.MetaData
{
    public class TempMetaDataService : IMetaDataService
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

        public Task<MetaDataObject> LoadMetaData(Source source)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(1500);

                var stringType = new Type("class String");

                var firstNameMember = new Member(
                    "(property) public String FirstName",
                    new List<Type> {stringType}
                );

                var lastNameMember = new Member(
                    "(property) public String LastName",
                    new List<Type> {stringType}
                );

                var relatedPersonField = new Member(
                    "(field) private Person _relatedPerson",
                    new List<Type>());

                var personType = new Type(
                    "public class Person",
                    new List<Member> {firstNameMember, lastNameMember, relatedPersonField}
                );

                relatedPersonField.RelatedTypes.Add(personType);

                var namespaceObject = new Namespace("Project", new List<Type> {personType});

                return new MetaDataObject(
                    "Project.dll",
                    new List<Namespace> {namespaceObject}
                );
            });
        }

        public Task<bool> SaveMetaData(Source metaDataSource)
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