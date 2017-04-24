using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AfekaTorrentServerConsole.EF.Repository
{
    public interface IFilesRepository
    {
        List<AfekaTorrentServerConsole.EF.File> SearchAvaiableFiles(string fileName);
        void AddFiles(List<AfekaTorrentServerConsole.EF.File> FilesList);
        List<AfekaTorrentServerConsole.EF.File> GetAllFiles();

        


    }
}
