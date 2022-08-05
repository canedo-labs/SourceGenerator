using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace SourceGenerator.Structures
{
    public class BuilderFinder : BaseFinder, ISyntaxReceiver
    {
        public BuilderFinder()
        {
            ClassDeclarations = new List<ClassDeclarationSyntax>();
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax builder 
                && builder.Identifier.ValueText.EndsWith("Builder"))
            {
                ClassDeclarations.Add(builder);
            }
        }
    }
}
