namespace VctoonCore.Resources;

public class Tag : Entity<Guid>
{
    public string Name { get; protected set; }

    // public string Color { get; set; }
    
    public void SetName(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Name = name;
    }
}