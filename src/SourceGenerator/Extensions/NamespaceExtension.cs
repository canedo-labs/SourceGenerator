using Microsoft.CodeAnalysis;
using System.Linq;

namespace SourceGenerator.Extensions
{
    public static class NamespaceExtension
    {
        public static INamespaceSymbol FirstOrDefaultBySegment(this INamespaceSymbol namespaceSymbol, string namespaceSegment) 
        {
            return namespaceSymbol.GetNamespaceMembers().FirstOrDefault(n => n.Name.Equals(namespaceSegment));
        }
    }
}
