using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using SourceGenerator.Services;
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
            var builderMembers = 
                new NamespaceService(context, "SourceGeneratorTest.Builders")
                .GetNamespaceMember()
                .GetTypeMembers();
            var typeNamespaceService = new NamespaceService(context, "SourceGenerator.Core");


            foreach (var builderMember in builderMembers)
            {
                // Finders
                var typeName = builderMember.MetadataName.Replace("Builder", "");
                var srcType = typeNamespaceService.GetTypeByClassName(typeName);
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
                    .AppendLine(Tab(1) + FullClassName(typeName))
                    .AppendLine(Tab(1) + "{")
                    .AppendLine(Tab(2) + $"public static {typeName} CreateDefault([Optional] {typeName} src)")
                    .AppendLine(Tab(2) + "{")
                    .AppendLine(Tab(3) + $"return src ?? new {typeName}();")
                    .AppendLine(Tab(2) + "}");

                foreach (IPropertySymbol property in properties)
                {
                    codeGen
                        .AppendLine("")
                        .AppendLine(Tab(2) + $"public static {typeName} With{property.Name}(this {typeName} src, {property.Type.Name} value)")
                        .AppendLine(Tab(2) + "{")
                        .AppendLine(Tab(3) + $"src.{property.Name} = value;")
                        .AppendLine(Tab(3) + "return src;")
                        .AppendLine(Tab(2) + "}");
                }

                codeGen
                    .AppendLine(Tab(1) + "}")
                    .AppendLine("}");

                // Creation
                context.AddSource(ClassName(typeName), SourceText.From(codeGen.ToString(), Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context) { }

        private string ClassName(string name) => name + "Builder";

        private string FullClassName(string name) => "public static partial class " + ClassName(name);

        private string Tab(int times) => new string(' ', 4 * times);
    }
}
