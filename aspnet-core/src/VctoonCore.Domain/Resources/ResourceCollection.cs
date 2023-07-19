namespace VctoonCore.Resources;

public class ResourceCollection : AggregateRoot<Guid>
{

    public string Name { get; protected set; }

    public virtual ICollection<Comic> Comics { get; } = new List<Comic>();
    public virtual ICollection<Video> Videos { get; } = new List<Video>();

    public void SetName(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Name = name;
    }
}
