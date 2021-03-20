using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
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
        public void CanGenerateDefaultType()
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

            result.AssertGeneratedFile("T.ValueItem.Generated.cs", tree =>
            {
                var treeText = tree.GetText().ToString();

                // Make sure we created a GUID generator.
                Assert.Contains("private readonly Guid _backingId;", treeText);

                // Check that the typeconverter gets added.
                Assert.Contains("[TypeConverter(typeof(ValueItemTypeConverter))]", treeText);
            });

            result.AssertGeneratedFile("T.ValueItem.TypeConverter.cs");
        }

        [Fact]
        public void CanGenerateIntType()
        {
            var inputCompilation = CreateCompilation(@"

using TypedIds;

namespace T
{
    [TypedId(IdBackingType.Int)]
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

            result.AssertGeneratedFile("T.ValueItem.Generated.cs", tree =>
            {
                var treeText = tree.GetText().ToString();

                // Make sure we created an int generator.
                Assert.Contains("private readonly int _backingId;", treeText);

                // Check that the typeconverter gets added.
                Assert.Contains("[TypeConverter(typeof(ValueItemTypeConverter))]", treeText);
            });

            result.AssertGeneratedFile("T.ValueItem.TypeConverter.cs");
        }

        [Fact]
        public void CanGenerateLongType()
        {
            var inputCompilation = CreateCompilation(@"

using TypedIds;

namespace T
{
    [TypedId(IdBackingType.Long)]
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

            result.AssertGeneratedFile("T.ValueItem.Generated.cs", tree =>
            {
                var treeText = tree.GetText().ToString();

                // Make sure we created an int generator.
                Assert.Contains("private readonly long _backingId;", treeText);

                // Check that the typeconverter gets added.
                Assert.Contains("[TypeConverter(typeof(ValueItemTypeConverter))]", treeText);
            });

            result.AssertGeneratedFile("T.ValueItem.TypeConverter.cs");
        }


        [Fact]
        public void CanGenerateStringType()
        {
            var inputCompilation = CreateCompilation(@"

using TypedIds;

namespace T
{
    [TypedId(IdBackingType.String)]
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

            result.AssertGeneratedFile("T.ValueItem.Generated.cs", tree =>
            {
                var treeText = tree.GetText().ToString();

                // Make sure we created an int generator.
                Assert.Contains("private readonly string _backingId;", treeText);

                // Check that the typeconverter gets added.
                Assert.Contains("[TypeConverter(typeof(ValueItemTypeConverter))]", treeText);
            });

            result.AssertGeneratedFile("T.ValueItem.TypeConverter.cs");
        }



        [Fact]
        public void CanRecoverFromBadAttributeState()
        {
            var inputCompilation = CreateCompilation(@"

using TypedIds;

namespace T
{
    [TypedId(]
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

            result.AssertGeneratedFile("T.ValueItem.Generated.cs", tree =>
            {
                var treeText = tree.GetText().ToString();

                // Make sure we created an int generator.
                Assert.Contains("private readonly Guid _backingId;", treeText);

                // Check that the typeconverter gets added.
                Assert.Contains("[TypeConverter(typeof(ValueItemTypeConverter))]", treeText);
            });

            result.AssertGeneratedFile("T.ValueItem.TypeConverter.cs");
        }

        [Fact]
        public void CanRecoverFromDuplicateTypes()
        {
            var inputCompilation = CreateCompilation(@"

using TypedIds;

namespace T
{
    [TypedId]
    public partial struct ValueItem
    {   
    } 

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

            result.AssertGeneratedFile("T.ValueItem.Generated.cs", tree =>
            {
                var treeText = tree.GetText().ToString();

                // Make sure we created an int generator.
                Assert.Contains("private readonly Guid _backingId;", treeText);

                // Check that the typeconverter gets added.
                Assert.Contains("[TypeConverter(typeof(ValueItemTypeConverter))]", treeText);
            });
        }

        private static Compilation CreateCompilation(string source)
            => CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
    }
}
