using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerator.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceGenerator.Extensions
{
    public static class GenExecContextExtension
    {
        public static TFinder GetFinder<TFinder>(this GeneratorExecutionContext context) where TFinder : BaseFinder 
        {
            return context.SyntaxReceiver as TFinder;
        }

        public static INamespaceSymbol GetSymbolByNamespaceName(this GeneratorExecutionContext context, string namespaceName)
        {
            var namespaceSegments = namespaceName.Split('.');

            return context.Compilation.GlobalNamespace.GetNamespaceMember(namespaceSegments);
        }

        public static INamespaceSymbol GetNamespaceMember(this INamespaceSymbol namespaceSymbol, params string[] namespaceSegments)
        {
            Array.ForEach(namespaceSegments, namespaceSegment => 
                namespaceSymbol = namespaceSymbol.GetNamespaceMembers().FirstOrDefault(symbol => symbol.Name.Equals(namespaceSegment)));

            return namespaceSymbol;
        }
        
        public static ISymbol GetSymbol(this GeneratorExecutionContext context, ClassDeclarationSyntax classDeclarationSyntax) 
        {
            return context.Compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree).GetDeclaredSymbol(classDeclarationSyntax);
        }

        public static IEnumerable<ISymbol> GetDeclaredSymbols(this BaseFinder finder, GeneratorExecutionContext context)
        {
            return
                finder.ClassDeclarations
                .Select(declaration => context.GetSymbol(declaration));
        }
    }
}
