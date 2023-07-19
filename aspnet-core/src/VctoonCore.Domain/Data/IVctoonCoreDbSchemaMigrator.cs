using System.Threading.Tasks;

namespace VctoonCore.Data;

public interface IVctoonCoreDbSchemaMigrator
{
    Task MigrateAsync();
}
