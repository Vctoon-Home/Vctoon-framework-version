namespace VctoonCore.Resources;

/// <summary>
/// 
/// </summary>
public static class ResourceEfCoreQueryableExtensions
{
    public static IQueryable<Comic> IncludeDetails(this IQueryable<Comic> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
                .Include(x => x.Chapters)
            // .Include(x => x.xxx) // TODO: AbpHelper generated
            ;
    }

    public static IQueryable<ComicChapter> IncludeDetails(this IQueryable<ComicChapter> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
                .Include(q => q.Images)
            ;
    }

    public static IQueryable<Video> IncludeDetails(this IQueryable<Video> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
            // .Include(x => x.xxx) // TODO: AbpHelper generated
            ;
    }
}