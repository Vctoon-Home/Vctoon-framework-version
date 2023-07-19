namespace VctoonCore.Permissions;

public static class VctoonCorePermissions
{
    public const string GroupName = "VctoonCore";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
    /// <summary>
    /// 
    /// </summary>
    public class Library
    {
        public const string Default = GroupName + ".Library";
        public const string Update = Default + ".Update";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}
