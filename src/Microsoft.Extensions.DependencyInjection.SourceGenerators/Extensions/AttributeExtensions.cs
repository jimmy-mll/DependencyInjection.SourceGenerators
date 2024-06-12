using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection.SourceGenerators.Models;

namespace Microsoft.Extensions.DependencyInjection.SourceGenerators.Extensions;

internal static class AttributeExtensions
{
    public static ServiceData ExtractServiceData(this AttributeData attribute)
    {
        var (symbolName, contractName) = attribute.AttributeClass!.TypeArguments.Length switch
        {
            0 => (null, null),
            1 => (attribute.AttributeClass!.TypeArguments[0].ToDisplayString(), null),
            2 => (attribute.AttributeClass!.TypeArguments[1].ToDisplayString(), attribute.AttributeClass!.TypeArguments[0].ToDisplayString()),
            _ => (null, null)
        };
        
        var key = attribute
            .ConstructorArguments
            .FirstOrDefault()
            .Value?
            .ToString();
        
        return new ServiceData(symbolName, contractName, key);
    }
}