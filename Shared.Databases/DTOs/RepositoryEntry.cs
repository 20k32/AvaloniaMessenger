namespace Shared.Databases.DTOs;

public abstract class RepositoryEntry
{
    public string Id { get; set; } = null!;
    public string UserName { get; protected init; } = null!;
    public abstract void CopyValuesTo(RepositoryEntry entry);
}