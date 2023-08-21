namespace VctoonCore.Libraries;

/// <summary>
/// 
/// </summary>
public static class LibraryEfCoreQueryableExtensions
{
    public static IQueryable<Library> IncludeDetails(this IQueryable<Library> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
                .Include(e => e.Paths)
            // .Include(x => x.xxx) // TODO: AbpHelper generated
            ;
    }

    public static IQueryable<LibraryPath> IncludeDetails(this IQueryable<LibraryPath> queryable, bool include = true)
    {
        if (!include)
        {
            return queryable;
        }

        return queryable
                .Include(x=> x.Files)
            // .Include(x => x.xxx) // TODO: AbpHelper generated
            ;
    }
}