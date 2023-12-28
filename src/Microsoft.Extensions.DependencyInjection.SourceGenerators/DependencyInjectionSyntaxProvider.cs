using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection.SourceGenerators.Extensions;
using Microsoft.Extensions.DependencyInjection.SourceGenerators.Models;
using Microsoft.Extensions.DependencyInjection.SourceGenerators.Models.Generated;

namespace Microsoft.Extensions.DependencyInjection.SourceGenerators;

public static class DependencyInjectionSyntaxProvider
{
    public static bool Predicate(SyntaxNode node, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        if (node is not ClassDeclarationSyntax classDeclaration)
            return false;

        var attribute = classDeclaration
            .AttributeLists
            .SelectMany(x => x.Attributes)
            .FirstOrDefault(x => x.Name.ToString().Equals(SingletonAttribute.ShortName) ||
                                 x.Name.ToString().Equals(ScopedAttribute.ShortName) ||
                                 x.Name.ToString().Equals(TransientAttribute.ShortName));

        return attribute is not null;
    }
    
    public static Service Transform(GeneratorSyntaxContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var symbol = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(context.Node)!;

        var attribute = symbol
            .GetAttributes()
            .First(x => x.AttributeClass!.Name.Equals(SingletonAttribute.Name) ||
                        x.AttributeClass!.Name.Equals(ScopedAttribute.Name) ||
                        x.AttributeClass!.Name.Equals(TransientAttribute.Name));

        var (symbolName, contractName, key) = attribute.ExtractServiceData();
        
        symbolName ??= symbol.ToDisplayString();
        contractName ??= symbol.Interfaces.FirstOrDefault()?.ToDisplayString();

        var lifetime = attribute.AttributeClass!.Name switch
        {
            SingletonAttribute.Name => ServiceLifetime.Singleton,
            ScopedAttribute.Name => ServiceLifetime.Scoped,
            TransientAttribute.Name => ServiceLifetime.Transient,
            _ => ServiceLifetime.Singleton
        };

        return new Service(symbolName, lifetime, contractName, key);
    }
}