using Microsoft.Extensions.DependencyInjection;
using VctoonCore.Data;
using Volo.Abp.DependencyInjection;

namespace VctoonCore.EntityFrameworkCore;

public class EntityFrameworkCoreVctoonCoreDbSchemaMigrator
    : IVctoonCoreDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreVctoonCoreDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the VctoonCoreDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<VctoonCoreDbContext>()
            .Database
            .MigrateAsync();
    }
}
