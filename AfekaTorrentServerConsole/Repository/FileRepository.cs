using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AfekaTorrentServerConsole.EF.Repository
{
    class FileRepository
    {
        private AfekaTorrentEntitiesContext _AfekaTorrentObjectContext;
        public FileRepository(IUnitOfWork unitOfWork)
        {
            
            _AfekaTorrentObjectContext = unitOfWork as AfekaTorrentEntitiesContext;
        }
        public List<AfekaTorrentServerConsole.EF.File> SearchAvaiableFiles(string fileName)
        {
            var filesList = from files in _AfekaTorrentObjectContext.Files
                            join peers in _AfekaTorrentObjectContext.Peers on files.PeerID equals peers.PeerID
                            where files.FileName.Contains(fileName)
                            select new {files,peers };
            List<AfekaTorrentServerConsole.EF.File> List = new List<File>();
            foreach (var item in filesList)
            {
                File file = new File();
                file.FileName = item.files.FileName;
                file.FileSize = item.files.FileSize;
                file.FileType = item.files.FileType;
                file.PeerHostName = item.peers.PeerHostName;
                file.PeerID = item.peers.PeerID;
                file.OwnedBy = item.files.OwnedBy; ///
                List.Add(file);
            }
            return List;
        }

        public void AddFiles(List<AfekaTorrentServerConsole.EF.File> FilesList)
        {
            try
            {
                foreach (AfekaTorrentServerConsole.EF.File file in FilesList)
                {
                    _AfekaTorrentObjectContext.Files.AddObject(file);
                }
            }
            catch (Exception exp)
            {
                throw new Exception(exp.InnerException.Message);
            }
        }

        public void AddPeer(AfekaTorrentServerConsole.EF.Peer Peer)
        {
            try
            {
                _AfekaTorrentObjectContext.Peers.AddObject(Peer);
            }
            catch (Exception exp)
            {
                throw new Exception(exp.InnerException.Message);
            }
        }

        public void Save()
        {
            _AfekaTorrentObjectContext.Save();            
        }
    
        public List<File> GetAllFiles()
        {
            var filesList = from files in _AfekaTorrentObjectContext.Files
                            join peers in _AfekaTorrentObjectContext.Peers on files.PeerID equals peers.PeerID
                            select new { files, peers };
            List<AfekaTorrentServerConsole.EF.File> List = new List<File>();
            foreach (var item in filesList)
            {
                File file = new File();
                file.FileName = item.files.FileName;
                file.FileSize = item.files.FileSize;
                file.FileType = item.files.FileType;
                
                file.PeerHostName = item.peers.PeerHostName;
                file.PeerID = item.peers.PeerID;
                file.OwnedBy = item.files.OwnedBy; ///
                List.Add(file);
            }
            return List;
        }

       

    }
    
}

