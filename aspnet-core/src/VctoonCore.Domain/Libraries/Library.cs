namespace VctoonCore.Libraries;

public class Library : AggregateRoot<Guid>
{
    public string Name { get; protected set; }
    public virtual List<LibraryPath> Paths { get; internal set; } = new List<LibraryPath>();

    protected internal void SetName(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Name = name;
    }

    protected Library()
    {
    }

    public Library(
        Guid id,
        string name
    ) : base(id)
    {
        Name = name;
    }
}