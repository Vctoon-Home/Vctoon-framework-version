using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VctoonClient.Storages.Base;

public class StorageConverter : JsonConverter
{
    private static AppStorageResolver resolver = new AppStorageResolver();

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        Type type = value.GetType();
                 
        if (type.GetSingleAttributeOrNull<StorageIgnoreAttribute>() != null)
            return;

        writer.WriteStartObject();

        List<MemberInfo> members = resolver.GetRuleMembers(type).ToList();

        // 检查类是否有 StorageEncryptionAttribute 特性
        bool isEncryptClass = type.GetCustomAttribute(typeof(StorageEncryptAttribute)) != null;

        foreach (var member in members)
        {
            // 检查属性是否有 StorageIgnoreAttribute 特性
            if (member.GetCustomAttribute(typeof(StorageIgnoreAttribute)) != null)
                continue;

            string memberName = member.Name;
            // object memberValue = member.GetValue(value);
            object memberValue = resolver.GetMemberValueOnConvertor(member, value, isEncryptClass);


            writer.WritePropertyName(memberName);
            writer.WriteValue(memberValue);
        }

        writer.WriteEndObject();
    }


    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject obj = JObject.Load(reader);
        var result = FormatterServices.GetUninitializedObject(objectType);


        if (objectType.GetSingleAttributeOrNull<StorageIgnoreAttribute>() != null)
            return result;


        // 检查类是否有 StorageEncryptionAttribute 特性
        bool isEncryptClass = objectType.GetCustomAttribute(typeof(StorageEncryptAttribute)) != null;

        var members = resolver.GetRuleMembers(objectType).ToList();
        foreach (var member in members)
        {
            var ignoreAttribute = member.GetCustomAttribute(typeof(StorageIgnoreAttribute));
            if (ignoreAttribute != null)
                continue;

            var storageAttribute = member.GetCustomAttribute(typeof(StorageAttribute));

            if (storageAttribute != null || obj[member.Name] != null)
            {
                resolver.SetMemberValueOnConvertor(member, result, obj[member.Name], isEncryptClass);
            }
        }

        return result;
    }


    public override bool CanConvert(Type objectType)
    {
        return true;
    }
}