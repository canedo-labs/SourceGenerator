using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace SourceGenerator.Finders
{
    public static class LazyNamespaceFinder
    {
        public static INamespaceOrTypeSymbol Find(INamespaceSymbol namespaceSymbol, string name)
        {
            var symbol = namespaceSymbol.GetMembers(name).FirstOrDefault();

            if (symbol != null)
            {
                return symbol;
            }

            var nms = namespaceSymbol.GetNamespaceMembers();

            foreach (var nm in nms)
            {
                var nmSymbol = nm.GetMembers(name).FirstOrDefault();

                if (nmSymbol != null)
                {
                    return nmSymbol;
                }

                var nmSymbolSearch = Find(nm, name);

                if (nmSymbolSearch != null)
                {
                    return nmSymbolSearch;
                }
            }

            throw new KeyNotFoundException(name + " not found");
        }
    }
}
