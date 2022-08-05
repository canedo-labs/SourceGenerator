using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace SourceGenerator.Structures
{
    public abstract class BaseFinder
    {
        public IList<ClassDeclarationSyntax> ClassDeclarations { get; protected set; }
    }
}
