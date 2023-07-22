using VctoonCore.Enums;

namespace VctoonCore.Libraries;

public class Library : AggregateRoot<Guid>
{
    public string Name { get; protected set; }
    public virtual List<LibraryPath> Paths { get; internal set; } = new List<LibraryPath>();

    public LibraryType LibraryType { get; init; }

    internal protected void SetName(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Name = name;
    }

    protected Library()
    {
    }

    public Library(
        Guid id,
        string name,
        LibraryType libraryType
    ) : base(id)
    {
        Name = name;
    }
}
