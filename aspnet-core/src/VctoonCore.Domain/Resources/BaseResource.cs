using System.IO;

namespace VctoonCore.Resources;

public abstract class BaseResource : AggregateRoot<Guid>
{
    protected BaseResource()
    {
    }

    public BaseResource(Guid id,
        string title,
        Guid libraryId,
        double rate = 0,
        string description = "",
        string author = "",
        int readCount = 0) : base(id)
    {
        SetTitle(title);
        SetRate(rate);
        SetDescription(description);
        SetAuthor(author);
        SetReadCount(readCount);
        LibraryId = libraryId;
    }

    public string Title { get; protected set; }

    public double Rate { get; protected set; }

    public string Description { get; protected set; }

    public string CoverPath { get; protected set; }

    public string Author { get; protected set; }


    public int ReadCount { get; protected set; }

    public Guid LibraryId { get; internal set; }

    public virtual ICollection<Tag> Tags { get; } = new List<Tag>();


    public virtual void SetTitle(string title)
    {
        Check.NotNullOrWhiteSpace(title, nameof(title));
        Title = title;
    }

    public virtual void SetRate(double rate)
    {
        Check.Range(rate, nameof(rate), 0, 5);
        Rate = rate;
    }

    public virtual void SetDescription(string description)
    {
        Description = description;
    }

    public virtual void SetCoverPath(string coverPath)
    {
        Check.NotNullOrWhiteSpace(coverPath, nameof(coverPath));

        if (!File.Exists(coverPath))
            throw new Exception(@$"${nameof(coverPath)} is not valid path. {coverPath} is not exist.");
        CoverPath = coverPath;
    }

    public virtual void SetAuthor(string author)
    {
        Author = author;
    }

    public virtual void SetReadCount(int readCount)
    {
        Check.Range(readCount, nameof(readCount), 0, int.MaxValue);
        ReadCount = readCount;
    }

    public virtual void AddTag(Tag tag)
    {
        Check.NotNull(tag, nameof(tag));

        if (Tags.Any(x => x.Name == tag.Name || x.Id == tag.Id))
        {
            throw new Exception(@$"{nameof(tag)} is already exist.");
        }

        Tags.Add(tag);
    }

    public virtual void RemoveTag(Tag tag)
    {
        Check.NotNull(tag, nameof(tag));
        Tags.RemoveAll(t => t.Name == tag.Name || t.Id == tag.Id);
    }
}