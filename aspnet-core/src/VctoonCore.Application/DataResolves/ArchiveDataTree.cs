using System.Collections.Generic;
using SharpCompress.Archives;

namespace VctoonCore.DataResolves;

public class ArchiveDataTree
{
    public string Key { get; set; }
    public IArchiveEntry[] Entries { get; set; }
    public List<ArchiveDataTree> Children { get; set; } = new List<ArchiveDataTree>();
}