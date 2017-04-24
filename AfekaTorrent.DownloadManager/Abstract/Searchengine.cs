using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace AfekaTorrent.DownloadManager.Abstract
{
    class Searchengine:ISearchEngine
    {
        public List<Entities.File> Search(string searchPattern)
        {
            FileServer.FilesServiceClient fileServiceClient = new FileServer.FilesServiceClient();
            
            List<Entities.File> filesList = new List<File>();
            foreach (var file in fileServiceClient.SearchAvaiableFiles(searchPattern))
            {
                Entities.File currentFile = new File();
                currentFile.FileName = file.FileName;
                currentFile.FileSize = file.FileSize;
                currentFile.FileType = file.FileType;
                currentFile.PeerID = file.PeerID;
                currentFile.PeerHostName = file.PeerHostName;
                currentFile.OwnedBy = file.OwnedBy;
                filesList.Add(currentFile);
            }
            return filesList;
        

        }

        public List<FileSearchResult> SearchByFileHashCode(string hashCode)
        {
            return null;
        }
    }
}
