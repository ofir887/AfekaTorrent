using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AfekaTorrentServerConsole.EF.Repository;
using AfekaTorrentServerConsole.EF;
using Entities;
using System.ServiceModel;

namespace AfekaTorrentServerConsole.WCFServices
{
    [ServiceContract]
    public class FilesService
    {
        private AfekaTorrentEntitiesContext _AfekaTorrentObjectContext = new AfekaTorrentEntitiesContext();
        [OperationContract]
        public void AddFiles(List<Entities.File> FilesList, Entities.Peer peer)
        {
            FileRepository fileRepository = new FileRepository(_AfekaTorrentObjectContext as AfekaTorrentServerConsole.IUnitOfWork);
            this.AddPeer(externalPeerToEFPeer(peer));
            fileRepository.AddFiles(externalFileToEFFile(FilesList));

            SaveFile();
        }
        [OperationContract]
        public void AddPeer(AfekaTorrentServerConsole.EF.Peer Peer)
        {
            FileRepository fileRepository = new FileRepository(_AfekaTorrentObjectContext as AfekaTorrentServerConsole.IUnitOfWork);
            fileRepository.AddPeer(Peer);

        }
        [OperationContract]
        public List<Entities.File> SearchAvaiableFiles(string fileName, Guid userId)
        {
            FileRepository fileRepository = new FileRepository(_AfekaTorrentObjectContext as AfekaTorrentServerConsole.IUnitOfWork);
            return internalFileToEntityFile(fileRepository.SearchAvaiableFiles(fileName));
        }
        [OperationContract]
        public List<Entities.File> GetAllFiles()
        {
            FileRepository fileRepository = new FileRepository(_AfekaTorrentObjectContext as AfekaTorrentServerConsole.IUnitOfWork);
            return internalFileToEntityFile(fileRepository.GetAllFiles());
        }



        public void SaveFile()
        {
            _AfekaTorrentObjectContext.Save();
        }

        private EF.Peer externalPeerToEFPeer(Entities.Peer peer)
        {
            EF.Peer EFPeer = new EF.Peer();
            EFPeer.PeerHostName = peer.PeerHostName;
            EFPeer.PeerID = peer.PeerID;
            
            return EFPeer;
        }

        private List<AfekaTorrentServerConsole.EF.File> externalFileToEFFile(List<Entities.File> fileList)
        {
            List<AfekaTorrentServerConsole.EF.File> nativeFileTypeList = new List<AfekaTorrentServerConsole.EF.File>();
            foreach (var file in fileList)
            {
                EF.File EFFile = new EF.File();
                EFFile.FileID = Guid.NewGuid();
                EFFile.FileName = file.FileName;
                EFFile.FileSize = file.FileSize;
                EFFile.FileType = file.FileType;
                EFFile.PeerID = file.PeerID;
                EFFile.PeerHostName = file.PeerHostName;
                EFFile.OwnedBy = file.OwnedBy; ///
                nativeFileTypeList.Add(EFFile);
            }
            return nativeFileTypeList;
        }

        private List<Entities.File> internalFileToEntityFile(List<AfekaTorrentServerConsole.EF.File> fileList)
        {
            List<Entities.File> entityFileTypeList = new List<Entities.File>();
            foreach (var file in fileList)
            {
                Entities.File File = new Entities.File();
                File.FileName = file.FileName;
                File.FileSize = file.FileSize;
                File.FileType = file.FileType;
                File.PeerHostName = file.PeerHostName;
                File.PeerID = file.PeerID;
                File.OwnedBy = file.OwnedBy;///
                entityFileTypeList.Add(File);
            }
            return entityFileTypeList;
        }

        public List<Entities.File> SearchAvaiableFiles(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}