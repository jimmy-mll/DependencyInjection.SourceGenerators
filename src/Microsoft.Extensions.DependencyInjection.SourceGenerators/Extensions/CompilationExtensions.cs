using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection.SourceGenerators.Models;
using Microsoft.Extensions.DependencyInjection.SourceGenerators.Models.Generated;

namespace Microsoft.Extensions.DependencyInjection.SourceGenerators.Extensions;

internal static class CompilationExtensions
{
    public static IncrementalValueProvider<string> GetAssemblyName(this IncrementalValueProvider<Compilation> provider)
    {
        return provider.Select((compilation, cancellationToken) =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            return compilation.Assembly.Name switch
            {
                var assemblyName when assemblyName.Contains('.') => assemblyName.Split('.').Last(),
                var assemblyName => assemblyName
            };
        });
    }
    
    public static IncrementalValueProvider<ImmutableArray<Service>> GetAssemblyAttributes(this IncrementalValueProvider<Compilation> provider)
    {
        return provider.Select((compilation, cancellationToken) =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            var attributes =  compilation.Assembly.GetAttributes();
            
            var builder = ImmutableArray.CreateBuilder<Service>();
        
            foreach (var attribute in attributes)
            {
                var (symbolName, contractName, key) = attribute.ExtractServiceData();
            
                if (symbolName is null)
                    continue;
   
                var lifetime = attribute.AttributeClass!.Name switch
                {
                    SingletonAttribute.Name => ServiceLifetime.Singleton,
                    ScopedAttribute.Name => ServiceLifetime.Scoped,
                    TransientAttribute.Name => ServiceLifetime.Transient,
                    _ => ServiceLifetime.Singleton
                };

                builder.Add(new Service(symbolName, lifetime, contractName, key));
            }
        
            return builder.ToImmutable();
        });
    }
}