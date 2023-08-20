using System;
using Newtonsoft.Json;

namespace VctoonClient.Storages.Base;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class StorageAttribute : Attribute
{
}