using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using SourceGenerator.Extensions;
using SourceGenerator.Searches;
using SourceGenerator.Structures;
using System;
using System.Linq;
using System.Text;

namespace SourceGenerator.SourceGenerators
{
    [Generator]
    public class TestBuilderGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var symbols = context.GetFinder<BuilderFinder>().GetDeclaredSymbols(context);

            foreach (var symbol in symbols)
            {
                // Finders
                var className = symbol.MetadataName.Replace("Builder", "");
                var namespaceSymbol = context.GetSymbolByNamespaceName("SourceGenerator.Core");
                var classSymbol = LazySearch.Execute(namespaceSymbol, className);
                var classProperties = classSymbol.GetMembers().Where(m => m.Kind.Equals(SymbolKind.Property));

                // Customizations
                var codeGen = new StringBuilder();
                
                codeGen
                    .AppendLine("// Generated at " + DateTime.Now)
                    .AppendLine("")
                    .AppendLine($"using {classSymbol.ContainingNamespace};")
                    .AppendLine("using System.Runtime.InteropServices;")
                    .AppendLine("")
                    .AppendLine("namespace SourceGeneratorTest.Builders")
                    .AppendLine("{")
                    .AppendLine(Tab(1) + FullClassName(className))
                    .AppendLine(Tab(1) + "{")
                    .AppendLine(Tab(2) + $"public static {className} CreateDefault([Optional] {className} src)")
                    .AppendLine(Tab(2) + "{")
                    .AppendLine(Tab(3) + $"return src ?? new {className}();")
                    .AppendLine(Tab(2) + "}");

                foreach (IPropertySymbol property in classProperties)
                {
                    codeGen
                        .AppendLine("")
                        .AppendLine(Tab(2) + $"public static {className} With{property.Name}(this {className} src, {property.Type.Name} value)")
                        .AppendLine(Tab(2) + "{")
                        .AppendLine(Tab(3) + $"src.{property.Name} = value;")
                        .AppendLine(Tab(3) + "return src;")
                        .AppendLine(Tab(2) + "}");
                }

                codeGen
                    .AppendLine(Tab(1) + "}")
                    .AppendLine("}");

                // Creation
                context.AddSource(ClassName(className), SourceText.From(codeGen.ToString(), Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context) 
        {
            context.RegisterForSyntaxNotifications(() => new BuilderFinder());
        }

        private string ClassName(string name) => name + "Builder";

        private string FullClassName(string name) => "public static partial class " + ClassName(name);

        private string Tab(int times) => new string(' ', 4 * times);
    }
}
