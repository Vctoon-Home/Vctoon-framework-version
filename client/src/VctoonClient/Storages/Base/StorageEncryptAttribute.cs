using System;

namespace VctoonClient.Storages.Base;

/// <summary>
/// only support string type
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class StorageEncryptAttribute : Attribute
{
}