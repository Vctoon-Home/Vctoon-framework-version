using VctoonCore.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace VctoonCore.Permissions;

public class VctoonCorePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var libraryGroup = context.AddGroup(VctoonCorePermissions.LibraryGroupName);
        var libraryPermission =
            libraryGroup.AddPermission(VctoonCorePermissions.Library.Default, L("Permission:Library"));
        libraryPermission.AddChild(VctoonCorePermissions.Library.Create, L("Permission:Create"));
        libraryPermission.AddChild(VctoonCorePermissions.Library.Update, L("Permission:Update"));
        libraryPermission.AddChild(VctoonCorePermissions.Library.Delete, L("Permission:Delete"));

        var tagGroup = context.AddGroup(VctoonCorePermissions.TagGroupName);
        var tagPermission = tagGroup.AddPermission(VctoonCorePermissions.Tag.Default, L("Permission:Tag"));
        tagPermission.AddChild(VctoonCorePermissions.Tag.Create, L("Permission:Create"));
        tagPermission.AddChild(VctoonCorePermissions.Tag.Update, L("Permission:Update"));
        tagPermission.AddChild(VctoonCorePermissions.Tag.Delete, L("Permission:Delete"));

        var comicGroup = context.AddGroup(VctoonCorePermissions.ComicGroupName);
        var comicPermission = comicGroup.AddPermission(VctoonCorePermissions.Comic.Default, L("Permission:Comic"));
        comicPermission.AddChild(VctoonCorePermissions.Comic.Update, L("Permission:Update"));
        comicPermission.AddChild(VctoonCorePermissions.Comic.Delete, L("Permission:Delete"));

        var comicChapterGroup = context.AddGroup(VctoonCorePermissions.ComicChapterGroupName);
        var comicChapterPermission =
            comicChapterGroup.AddPermission(VctoonCorePermissions.ComicChapter.Default, L("Permission:ComicChapter"));
        comicChapterPermission.AddChild(VctoonCorePermissions.ComicChapter.Update, L("Permission:Update"));
        comicChapterPermission.AddChild(VctoonCorePermissions.ComicChapter.Delete, L("Permission:Delete"));

        var comicImageGroup = context.AddGroup(VctoonCorePermissions.ComicImageGroupName);
        var comicImagePermission =
            comicImageGroup.AddPermission(VctoonCorePermissions.ComicImage.Default, L("Permission:ComicImage"));
        comicImagePermission.AddChild(VctoonCorePermissions.ComicImage.Update, L("Permission:Update"));
        comicImagePermission.AddChild(VctoonCorePermissions.ComicImage.Delete, L("Permission:Delete"));

        var videoGroup = context.AddGroup(VctoonCorePermissions.VideoGroupName);
        var videoPermission = videoGroup.AddPermission(VctoonCorePermissions.Video.Default, L("Permission:Video"));
        videoPermission.AddChild(VctoonCorePermissions.Video.Update, L("Permission:Update"));
        videoPermission.AddChild(VctoonCorePermissions.Video.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<VctoonCoreResource>(name);
    }
}