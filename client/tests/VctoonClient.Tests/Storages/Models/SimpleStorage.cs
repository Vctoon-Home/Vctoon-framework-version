using System.Text.Json.Serialization;
using VctoonClient.Storages.Base;

namespace VctoonClient.Tests.Storages.Models;

public class CompObject
{
    public string Name { get; set; }
}

public class SimpleStorage : AppStorageBase
{
    public SimpleStorage(string savePath)
    {
        _savePath = savePath;
    }


    [Storage]
    private string StoragePrivateFieldStr;

    public void SetStoragePrivateFieldStr(string value)
    {
        StoragePrivateFieldStr = value;
    }

    public bool IsSameStoragePrivateFieldStr(SimpleStorage storage)
    {
        return StoragePrivateFieldStr == storage.StoragePrivateFieldStr;
    }

    [Storage]
    public string StoragePublicFieldStr;

    [Storage]
    private string StoragePrivateStr { get; set; }

    public void SetStoragePrivateStr(string value)
    {
        StoragePrivateStr = value;
    }

    public bool IsSameStoragePrivateStr(SimpleStorage storage)
    {
        return StoragePrivateStr == storage.StoragePrivateStr;
    }


    [StorageIgnore]
    public string StorageIgnorePublicStr { get; set; }

    [StorageEncrypt]
    private string StorageEncryptionPrivateStr { get; set; }

    public void SetStorageEncryptionPrivateStr(string value)
    {
        StorageEncryptionPrivateStr = value;
    }

    public bool IsSameStorageEncryptionPrivateStr(SimpleStorage storage)
    {
        return StorageEncryptionPrivateStr == storage.StorageEncryptionPrivateStr;
    }

    
    public bool IsSameStorageEncryptionPrivateStr(string str)
    {
        return StorageEncryptionPrivateStr == str;
    }


    public string StoragePublicStr { get; set; }

    public string? StoragePublicNullStr { get; set; }

    public int? StoragePublicNullInt { get; set; }

    public long? StoragePublicNullLong { get; set; }

    public float? StoragePublicNullFloat { get; set; }

    public double? StoragePublicNullDouble { get; set; }

    public decimal? StoragePublicNullDecimal { get; set; }

    public bool? StoragePublicNullBool { get; set; }

    public DateTime? StoragePublicNullDateTime { get; set; }

    public DateTimeOffset? StoragePublicNullDateTimeOffset { get; set; }

    public TimeSpan? StoragePublicNullTimeSpan { get; set; }

    public CompObject? StoragePublicNullCompObject { get; set; }

    public int StoragePublicInt { get; set; }

    public long StoragePublicLong { get; set; }

    public float StoragePublicFloat { get; set; }

    public double StoragePublicDouble { get; set; }

    public decimal StoragePublicDecimal { get; set; }

    public bool StoragePublicBool { get; set; }

    public DateTime StoragePublicDateTime { get; set; }

    public DateTimeOffset StoragePublicDateTimeOffset { get; set; }

    public TimeSpan StoragePublicTimeSpan { get; set; }

    public CompObject StoragePublicCompObject { get; set; }


    [StorageEncrypt]
    public string StorageEncryptionPublicStr { get; set; }

    [StorageEncrypt]
    public int StorageEncryptionPublicInt { get; set; }
}