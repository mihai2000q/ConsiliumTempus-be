using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ConsiliumTempus.Api.Controllers;
using ConsiliumTempus.Application.User.Queries.Get;
using ConsiliumTempus.Domain.User;
using ConsiliumTempus.Infrastructure.Persistence.Repository;
using Assembly = System.Reflection.Assembly;

namespace ConsiliumTempus.ArchitectureTests.Core;

public class BaseArchitectureTest
{
    private static readonly Assembly ApiAssembly = typeof(UserController).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(GetUserQuery).Assembly;
    private static readonly Assembly DomainAssembly = typeof(UserAggregate).Assembly;
    private static readonly Assembly InfrastructureAssembly = typeof(UserRepository).Assembly;

    protected static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            ApiAssembly,
            ApplicationAssembly,
            DomainAssembly,
            InfrastructureAssembly)
        .Build();

    protected static readonly IObjectProvider<IType> ApiLayer = ArchRuleDefinition
        .Types()
        .That()
        .ResideInAssembly(ApiAssembly);
    protected static readonly IObjectProvider<IType> ApplicationLayer = ArchRuleDefinition
        .Types()
        .That()
        .ResideInAssembly(ApplicationAssembly);
    protected static readonly IObjectProvider<IType> DomainLayer = ArchRuleDefinition
        .Types()
        .That()
        .ResideInAssembly(DomainAssembly);
    protected static readonly IObjectProvider<IType> InfrastructureLayer = ArchRuleDefinition
        .Types()
        .That()
        .ResideInAssembly(InfrastructureAssembly);
}