using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
namespace AfekaTorrentServerConsole.EF
{
    class AfekaTorrentEntitiesContext:ObjectContext,IUnitOfWork
    {
        private ObjectSet<AfekaTorrentServerConsole.EF.File> _files;
        private ObjectSet<AfekaTorrentServerConsole.EF.Peer> _peers;
        private ObjectSet<AfekaTorrentServerConsole.EF.User> _users;
        public AfekaTorrentEntitiesContext()
            : base("name=AfekaTorrentEntities", "AfekaTorrentEntities1")
        {
            _files = CreateObjectSet<AfekaTorrentServerConsole.EF.File>();
            _peers = CreateObjectSet<AfekaTorrentServerConsole.EF.Peer>();
            _users = CreateObjectSet<AfekaTorrentServerConsole.EF.User>();
        }

        public ObjectSet<AfekaTorrentServerConsole.EF.File> Files
        {
            get { return _files; }
        }

        public ObjectSet<AfekaTorrentServerConsole.EF.Peer> Peers
        {
            get { return _peers; }
        }
        public ObjectSet<AfekaTorrentServerConsole.EF.User> Users
        {
            get { return _users; }
        }

        public void Save()
        {
            try
            {
                SaveChanges();
            }
            catch (Exception exp)
            {
                throw new Exception(exp.InnerException.Message);
            }
        }
    }
}
