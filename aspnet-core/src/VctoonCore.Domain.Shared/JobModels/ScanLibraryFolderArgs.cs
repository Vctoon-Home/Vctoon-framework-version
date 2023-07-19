using System;

namespace VctoonCore.JobModels;

public class ScanLibraryFolderArgs
{
    public ScanLibraryFolderArgs(Guid libraryId)
    {
        LibraryId = libraryId;
    }

    public Guid LibraryId { get; set; }
}