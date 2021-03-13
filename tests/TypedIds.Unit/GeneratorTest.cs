using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using Xunit;

namespace TypedIds.Unit
{
    public class GeneratorTest
    {
        [Fact]
        public void DiagnosticRaisedForMissingPartial()
        {
            var inputCompilation = CreateCompilation(@"
            
using TypedIds;

namespace T
{
    [TypedId]
    public struct ValueItem
    {
    
    }
}

            ");

            var generator = new Generator();

            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

            var result = driver.GetRunResult();

            Assert.Collection(result.Diagnostics, el =>
            {
                Assert.Equal("TYPEDID001", el.Id);
            });
        }

        [Fact]
        public void CanGenerate()
        {
            var inputCompilation = CreateCompilation(@"

using TypedIds;

namespace T
{
    [TypedId]
    public partial struct ValueItem
    {
    
    }
}

            ");

            var generator = new Generator();

            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

            var result = driver.GetRunResult();

            Assert.Empty(result.Diagnostics);

            Assert.NotEmpty(result.GeneratedTrees);
        }

        private static Compilation CreateCompilation(string source)
            => CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
    }
}
