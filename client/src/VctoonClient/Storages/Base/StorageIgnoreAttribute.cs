using System;
using Newtonsoft.Json;

namespace VctoonClient.Storages.Base;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public class StorageIgnoreAttribute : Attribute
{
}