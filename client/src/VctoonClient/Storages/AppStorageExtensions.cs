using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VctoonClient.Storages.Apps;
using VctoonClient.Storages.Base;
using VctoonClient.Storages.Users;

namespace VctoonClient.Storages;

public static class AppStorageExtensions
{
    public static void AddAppStorages(this IServiceCollection services, IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            if (!typeof(IAppStorage).IsAssignableFrom(type))
            {
                throw new ArgumentException($"Type {type} is not assignable from {typeof(IAppStorage)}");
            }


            services.AddSingleton(type);
            services.AddSingleton(typeof(IAppStorage), s =>
            {
                IAppStorage? storage = s.GetRequiredService(type) as IAppStorage;
                AppStorageSavingHandler.OnApplicationExit += () => { storage?.SaveStorage(); };

                return storage;
            });
        }
    }

    public static void AddAppStorages(this IServiceCollection services)
    {
        var storageTypes = new List<Type>();

        // 获取正在执行的程序集
        var executingAssembly = Assembly.GetExecutingAssembly();

        // 添加正在执行的程序集
        var assemblies = new List<Assembly> {executingAssembly};

        // 添加所有依赖的程序集
        assemblies.AddRange(executingAssembly.GetReferencedAssemblies().Select(Assembly.Load));

        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IAppStorage).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    storageTypes.Add(type);
                }
            }
        }

        services.AddAppStorages(storageTypes);
    }
}