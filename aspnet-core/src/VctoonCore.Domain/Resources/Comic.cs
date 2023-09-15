namespace VctoonCore.Resources;

public class Comic : BaseResource
{
    protected Comic()
    {
    }


    public Comic(Guid id,
        string title,
        Guid libraryId,
        double rate = 0,
        string description = "",
        string author = "",
        int readCount = 0) : base(id, title, libraryId, rate,
        description, author, readCount)
    {
    }

    public virtual List<ComicChapter> Chapters { get; } = new List<ComicChapter>();


    public void AddChapter(ComicChapter chapter)
    {
        Check.NotNull(chapter, nameof(chapter));
        chapter.ComicId = Id;
        Chapters.Add(chapter);
    }

    public void RemoveChapter(ComicChapter chapter)
    {
        Check.NotNull(chapter, nameof(chapter));
        Chapters.Remove(chapter);
    }
}