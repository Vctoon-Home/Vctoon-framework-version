using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shouldly;
using VctoonClient.Tests.Storages.Models;

namespace VctoonClient.Tests.Storages;

public class StorageTests
{
    public StorageTests()
    {
    }

    [Fact]
    public void Should_Save_Storage()
    {
        var filePath = Path.Combine("Settings", "test.json");

        // Arrange
        var storage = new SimpleStorage(filePath);

        // set public members
        storage.StoragePublicBool = true;
        storage.StoragePublicCompObject = new CompObject {Name = "test"};
        storage.StoragePublicDateTime = DateTime.Now;
        storage.StoragePublicDateTimeOffset = DateTimeOffset.Now;
        storage.StoragePublicDecimal = 1.1m;
        storage.StoragePublicDouble = 1.1;
        storage.StoragePublicFloat = 1.1f;
        storage.StoragePublicInt = 1;
        storage.StoragePublicLong = 1;
        storage.StoragePublicStr = "test";
        storage.StoragePublicTimeSpan = TimeSpan.FromDays(1);

        storage.StorageEncryptionPublicStr = "test";
        storage.StorageEncryptionPublicInt = 5;

        storage.StoragePublicFieldStr = "test";

        // set private members
        storage.SetStorageEncryptionPrivateStr("test");
        storage.SetStoragePrivateStr("test");
        storage.SetStoragePrivateFieldStr("test");


        // Act
        storage.SaveStorage();

        // Assert
        var storage2 = new SimpleStorage(filePath);
        storage2.LoadStorage();


        // public items should be same
        storage2.StoragePublicFieldStr.ShouldBe(storage.StoragePublicFieldStr);

        storage2.StoragePublicBool.ShouldBe(storage.StoragePublicBool);
        storage2.StoragePublicCompObject.Name.ShouldBe(storage.StoragePublicCompObject.Name);
        storage2.StoragePublicDateTime.ShouldBe(storage.StoragePublicDateTime);
        storage2.StoragePublicDateTimeOffset.ShouldBe(storage.StoragePublicDateTimeOffset);
        storage2.StoragePublicDecimal.ShouldBe(storage.StoragePublicDecimal);
        storage2.StoragePublicDouble.ShouldBe(storage.StoragePublicDouble);
        storage2.StoragePublicFloat.ShouldBe(storage.StoragePublicFloat);
        storage2.StoragePublicInt.ShouldBe(storage.StoragePublicInt);
        storage2.StoragePublicLong.ShouldBe(storage.StoragePublicLong);
        storage2.StoragePublicStr.ShouldBe(storage.StoragePublicStr);
        storage2.StoragePublicTimeSpan.ShouldBe(storage.StoragePublicTimeSpan);

        // encryption items should be same
        storage2.StorageEncryptionPublicStr.ShouldBe(storage.StorageEncryptionPublicStr);
        storage2.StorageEncryptionPublicInt.ShouldBe(storage.StorageEncryptionPublicInt);
        storage2.IsSameStorageEncryptionPrivateStr(storage).ShouldBe(true);


        // physical file encryption data should be not same
        {
            var stream = new StreamReader(filePath);
            var jobj = JObject.Load(new JsonTextReader(stream));


            var storageEncryptionPublicStr = jobj["StorageEncryptionPublicStr"].Value<string>();
            storageEncryptionPublicStr.ShouldNotBe(storage.StorageEncryptionPublicStr);

            var storageEncryptionPrivateStr = jobj["StorageEncryptionPrivateStr"].Value<string>();
            storage.IsSameStorageEncryptionPrivateStr(storageEncryptionPrivateStr)
                .ShouldBe(false);

            var storageEncryptionPublicInt = jobj["StorageEncryptionPublicInt"].Value<int>();
            storageEncryptionPublicInt.ShouldBe(storage.StorageEncryptionPublicInt);

            // private items should be same
            storage2.IsSameStoragePrivateStr(storage).ShouldBe(true);
            storage2.IsSameStoragePrivateFieldStr(storage).ShouldBe(true);
            
            stream.Dispose();
        }


        // nullable properties should be same
        storage2.StoragePublicNullBool.ShouldBe(storage.StoragePublicNullBool);
        storage2.StoragePublicNullCompObject.ShouldBe(storage.StoragePublicNullCompObject);
        storage2.StoragePublicNullDateTime.ShouldBe(storage.StoragePublicNullDateTime);
        storage2.StoragePublicNullDateTimeOffset.ShouldBe(storage.StoragePublicNullDateTimeOffset);
        storage2.StoragePublicNullDecimal.ShouldBe(storage.StoragePublicNullDecimal);
        storage2.StoragePublicNullDouble.ShouldBe(storage.StoragePublicNullDouble);
        storage2.StoragePublicNullFloat.ShouldBe(storage.StoragePublicNullFloat);
        storage2.StoragePublicNullInt.ShouldBe(storage.StoragePublicNullInt);
        storage2.StoragePublicNullLong.ShouldBe(storage.StoragePublicNullLong);
        storage2.StoragePublicNullStr.ShouldBe(storage.StoragePublicNullStr);
        storage2.StoragePublicNullTimeSpan.ShouldBe(storage.StoragePublicNullTimeSpan);


        File.Delete(filePath);
    }
}