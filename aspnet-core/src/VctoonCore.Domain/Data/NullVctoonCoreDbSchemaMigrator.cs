using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace VctoonCore.Data;

/* This is used if database provider does't define
 * IVctoonCoreDbSchemaMigrator implementation.
 */
public class NullVctoonCoreDbSchemaMigrator : IVctoonCoreDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
