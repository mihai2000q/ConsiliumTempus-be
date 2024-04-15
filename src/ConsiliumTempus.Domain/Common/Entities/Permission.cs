using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.Entities;

public sealed class Permission : Entity<int>
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private Permission()
    {
    }

    private Permission(int id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; init; } = string.Empty;

    public static Permission Create(int id, string name)
    {
        return new Permission(id, name);
    }
}