using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceGenerator
{
    [Generator]
    public class TestBuilderGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var compilation = context.Compilation;
            var members =
                compilation.GlobalNamespace
                .GetNamespaceMembers().First(q => q.Name == "SourceGeneratorTest")
                .GetNamespaceMembers().First(q => q.Name == "Builders")
                .GetTypeMembers()
                .ToList();

            var targets = new List<INamedTypeSymbol>();
            
            foreach (var member in members)
            {
                // Finders
                var srcName = member.MetadataName.Replace("Builder", "");
                var srcType = compilation.GetTypeByMetadataName("SourceGeneratorTest.Models." + srcName);
                var properties = srcType.GetMembers().Where(m => m.Kind.Equals(SymbolKind.Property));

                // Customizations
                var codeGen = new StringBuilder();

                codeGen
                    .AppendLine("// Generated at " + DateTime.Now)
                    .AppendLine("")
                    .AppendLine("using SourceGeneratorTest.Models;")
                    .AppendLine("using System.Runtime.InteropServices;")
                    .AppendLine("")
                    .AppendLine("namespace SourceGeneratorTest.Builders")
                    .AppendLine("{")
                    .AppendLine(Tab(1) + FullClassName(srcName))
                    .AppendLine(Tab(1) + "{")
                    .AppendLine(Tab(2) + $"public static {srcName} CreateDefault([Optional] {srcName} src)")
                    .AppendLine(Tab(2) + "{")
                    .AppendLine(Tab(3) + $"return src ?? new {srcName}();")
                    .AppendLine(Tab(2) + "}");

                foreach (IPropertySymbol property in properties)
                {
                    codeGen
                        .AppendLine("")
                        .AppendLine(Tab(2) + $"public static {srcName} With{property.Name}(this {srcName} src, {property.Type.Name} value)")
                        .AppendLine(Tab(2) + "{")
                        .AppendLine(Tab(3) + $"src.{property.Name} = value;")
                        .AppendLine(Tab(3) + "return src;")
                        .AppendLine(Tab(2) + "}");
                }

                codeGen
                    .AppendLine(Tab(1) + "}")
                    .AppendLine("}");

                // Creation
                context.AddSource(ClassName(srcName), SourceText.From(codeGen.ToString(), Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context) { }

        private string ClassName(string name) => name + "Builder";

        private string FullClassName(string name) => "public static partial class " + ClassName(name);

        private string Tab(int times) => new string(' ', 4 * times);
    }
}
