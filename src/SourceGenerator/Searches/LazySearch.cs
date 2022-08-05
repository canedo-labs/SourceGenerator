using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace SourceGenerator.Searches
{
    public static class LazySearch
    {
        public static INamespaceOrTypeSymbol Execute(INamespaceSymbol namespaceSymbol, string name)
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

                var nmSymbolSearch = Execute(nm, name);

                if (nmSymbolSearch != null)
                {
                    return nmSymbolSearch;
                }
            }

            throw new KeyNotFoundException(name + " not found");
        }
    }
}
