using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace SourceGenerator.Structures
{
    public class NamespaceSymbolStructure
    {
        private readonly INamespaceSymbol _namespaceSymbol;

        public NamespaceSymbolStructure(INamespaceSymbol namespaceSymbol)
        {
            _namespaceSymbol = namespaceSymbol;

            NamedTypeSymbols = _namespaceSymbol.GetTypeMembers().ToDictionary(k => k.Name, v => v);
            NamespaceSymbolTrees = _namespaceSymbol.GetNamespaceMembers().Select(n => new NamespaceSymbolStructure(n)).ToList();
        }

        public IDictionary<string, INamedTypeSymbol> NamedTypeSymbols { get; private set; }
        public IList<NamespaceSymbolStructure> NamespaceSymbolTrees { get; private set; }

        public INamedTypeSymbol Search(string className)
        {
            if (NamedTypeSymbols.ContainsKey(className))
            {
                return NamedTypeSymbols[className];
            }

            foreach (var tree in NamespaceSymbolTrees)
            {
                var resultSearch = tree.Search(className);

                if (resultSearch is null) continue;

                return resultSearch;
            }

            return null;
        }
        public override string ToString()
        {
            return _namespaceSymbol.Name;
        }
    }
}
