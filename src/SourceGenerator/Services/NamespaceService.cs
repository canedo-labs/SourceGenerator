using Microsoft.CodeAnalysis;
using System;
using System.Text;
using SourceGenerator.Extensions;
using System.Collections.Immutable;
using SourceGenerator.Structures;

namespace SourceGenerator.Services
{
    public class NamespaceService
    {
        public NamespaceService(GeneratorExecutionContext context, string targetNamespace)
        {
            Context = context;
            TargetNamespace = targetNamespace;

            // Customs
            Compilation = Context.Compilation;
            GlobalNamespace = Compilation.GlobalNamespace;
            SegmentTargetNamespace = TargetNamespace.Split('.');
        }

        public GeneratorExecutionContext Context { get; private set; }
        public Compilation Compilation { get; private set; }
        public INamespaceSymbol GlobalNamespace { get; private set; }
        public string TargetNamespace { get; private set; }
        public string[] SegmentTargetNamespace { get; private set; }
        public NamespaceSymbolStructure NamespaceSymbolTree { get; set; }

        public INamespaceSymbol GetNamespaceMember() 
        {
            var namespaceMember = GlobalNamespace;

            Array.ForEach(SegmentTargetNamespace, s => namespaceMember = namespaceMember.FirstOrDefaultBySegment(s));

            return namespaceMember;
        }

        public INamedTypeSymbol GetTypeByClassName(string className) 
        {
            var namespaceMember = GetNamespaceMember();

            NamespaceSymbolTree = NamespaceSymbolTree ?? new NamespaceSymbolStructure(namespaceMember);

            return NamespaceSymbolTree.Search(className);
        }
    }
}
