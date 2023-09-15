namespace VctoonCore.Permissions;

public static class VctoonCorePermissions
{
    public const string GroupName = "VctoonCore";

    public const string LibraryGroupName = GroupName + ".Library";

    public class Library
    {
        public const string Default = LibraryGroupName;
        public const string Update = Default + ".Update";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public const string TagGroupName = GroupName + ".Tag";

    public class Tag
    {
        public const string Default = TagGroupName;
        public const string Update = Default + ".Update";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }

    public const string ComicGroupName = GroupName + ".Comic";

    public class Comic
    {
        public const string Default = ComicGroupName;
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public const string ComicChapterGroupName = GroupName + ".ComicChapter";

    public class ComicChapter
    {
        public const string Default = ComicChapterGroupName;
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public const string ComicImageGroupName = GroupName + ".ComicImage";

    public class ComicImage
    {
        public const string Default = ComicImageGroupName;
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }

    public const string VideoGroupName = GroupName + ".Video";

    public class Video
    {
        public const string Default = VideoGroupName;
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
    }
}