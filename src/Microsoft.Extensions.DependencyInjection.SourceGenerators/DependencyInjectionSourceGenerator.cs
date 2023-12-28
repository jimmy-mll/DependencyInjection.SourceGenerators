using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection.SourceGenerators.Extensions;
using Microsoft.Extensions.DependencyInjection.SourceGenerators.Models.Generated;

namespace Microsoft.Extensions.DependencyInjection.SourceGenerators;

[Generator(LanguageNames.CSharp)]
public sealed class DependencyInjectionSourceGenerator : IIncrementalGenerator
{
    public const string Namespace = "Microsoft.Extensions.DependencyInjection";
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource(ScopedAttribute.GlobalName, SourceText.From(ScopedAttribute.Source, Encoding.UTF8));
            ctx.AddSource(SingletonAttribute.GlobalName, SourceText.From(SingletonAttribute.Source, Encoding.UTF8));
            ctx.AddSource(TransientAttribute.GlobalName, SourceText.From(TransientAttribute.Source, Encoding.UTF8));
        });

        var assemblyName = context.CompilationProvider.GetAssemblyName();
        
        var assemblyAttributes = context.CompilationProvider.GetAssemblyAttributes();
        
        var syntax = context.SyntaxProvider.CreateSyntaxProvider(
            DependencyInjectionSyntaxProvider.Predicate,
            DependencyInjectionSyntaxProvider.Transform);

        var provider = syntax
            .Collect()
            .Combine(assemblyName)
            .Combine(assemblyAttributes);
        
        context.RegisterSourceOutput(provider, (ctx, pair) =>
        {
            var services = pair.Left.Left.Concat(pair.Right).ToImmutableArray();

            DependencyInjectionGenerator.Generate(ctx, pair.Left.Right, services);
        });
    }
}