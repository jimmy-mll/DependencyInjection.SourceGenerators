using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection.SourceGenerators.Models;

public sealed record Service(string Name, ServiceLifetime Lifetime, string? ContractName, string? Key)
{
    [MemberNotNullWhen(true, nameof(ContractName))]
    public bool HasContract =>
        ContractName is not null;
}