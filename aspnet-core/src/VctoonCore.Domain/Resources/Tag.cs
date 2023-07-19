namespace VctoonCore.Resources;

public class Tag : Entity<Guid>
{
    public string Name { get; protected set; }

    public void SetName(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Name = name;
    }
}