using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using DotDll.Model.Analysis;
using NUnit.Framework;

namespace DotDll.Tests.Model
{

    [TestFixture]
    public class ReflectionDllAnalyzerTest
    {
        private ReflectionDllAnalyzer _analyzer;

        [SetUp]
        public void SetUp()
        {
            var builder = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName("Test.Assembly"), AssemblyBuilderAccess.RunAndCollect
            );

            var moduleBuilder = builder.DefineDynamicModule("Test.Assembly.Module", true);

            var typeBuilder = moduleBuilder
                .DefineType("TestClass");

            var methodBuilder = typeBuilder.DefineMethod(
                "AnyMethod", 
                MethodAttributes.Public | MethodAttributes.Virtual,
                CallingConventions.Standard,
                typeof(string), 
                new[] {typeof(int)}
                );

            methodBuilder.GetILGenerator().Emit(OpCodes.Ret);

            typeBuilder.CreateType();

            _analyzer = new ReflectionDllAnalyzer((path) => typeBuilder.Assembly);
        }
        
        [Test]
        public void Analyze_CheckProperMetadataName()
        {
   
            var metadataInfo = _analyzer.Analyze("");

            Assert.AreEqual("Test.Assembly", metadataInfo.Name);
        }
    }
}