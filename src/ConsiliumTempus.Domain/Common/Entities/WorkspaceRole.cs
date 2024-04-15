using System.Diagnostics.CodeAnalysis;
using ConsiliumTempus.Domain.Common.Models;

namespace ConsiliumTempus.Domain.Common.Entities;

public sealed class WorkspaceRole : Enumeration<WorkspaceRole>
{
    public static readonly WorkspaceRole View = new(1, nameof(View))
    {
        Description = "This role can only read data"
    };

    public static readonly WorkspaceRole Member = new(2, nameof(Member))
    {
        Description = "This role can do most of the actions with some limitations"
    };

    public static readonly WorkspaceRole Admin = new(3, nameof(Admin))
    {
        Description = "This role can do everything"
    };

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private WorkspaceRole()
    {
    }

    private WorkspaceRole(int id, string name) : base(id, name)
    {
    }

    public string Description { get; init; } = string.Empty;
    public ICollection<Permission> Permissions { get; init; } = [];
}