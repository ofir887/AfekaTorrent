using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AfekaTorrent.DownloadManager.Abstract
{
    public interface ISearchEngine
    {
        List<Entities.File> Search(string searchPattern);
        List<FileSearchResult> SearchByFileHashCode(string hashCode);
    }
}
