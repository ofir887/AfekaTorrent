using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using System.Text;
using Entities;
using AfekaTorrentServerConsole.EF;

namespace AfekaTorrentServerConsole.Repository
{
    class UserRepository
    {
        private AfekaTorrentEntitiesContext _AfekaTorrentObjectContext;
        public UserRepository(IUnitOfWork unitOfWork)
        {

            _AfekaTorrentObjectContext = unitOfWork as AfekaTorrentEntitiesContext;
        }
        public void AddUser(EF.User user)
        {
            _AfekaTorrentObjectContext.Users.AddObject(user);
        }

        public Guid Login(string userName, string password)
        {
            var userList = this.GetAllUsers();
            var user = userList.Where(x => x.UserName == userName && x.Password == password).FirstOrDefault();
            return user != null ? user.UserID : Guid.Empty;

        }
        public List<EF.User> GetAllUsers()
        {
            var userList = from users in _AfekaTorrentObjectContext.Users

                           select new { users };
            List<AfekaTorrentServerConsole.EF.User> List = new List<EF.User>();
            foreach (var item in userList)
            {
                EF.User user = new EF.User();
                user.UserName = item.users.UserName;
                user.UserID = item.users.UserID;
                user.Password = item.users.Password;
                user.IsEnabled = item.users.IsEnabled;
                user.DownloadFolder = item.users.DownloadFolder;
                user.SharedFolder = item.users.SharedFolder;
                user.IsActive = item.users.IsActive;
                List.Add(user);
            }
            return List;
        }

        public void DeleteUser(Guid UserID)
        {
            EF.User user = _AfekaTorrentObjectContext.Users.Where(o => o.UserID == UserID).FirstOrDefault();
            _AfekaTorrentObjectContext.Users.DeleteObject(user);
            _AfekaTorrentObjectContext.SaveChanges();
        }

        public EF.User GetUser(Guid UserID)
        {
            return _AfekaTorrentObjectContext.Users.Where(o => o.UserID == UserID).FirstOrDefault();
        }

        public void EditUser(Entities.User user)
        {
            EF.User efUser = _AfekaTorrentObjectContext.Users.Where(o => o.UserID == user.UserID).FirstOrDefault();
            efUser.UserName = user.UserName;
            efUser.Password = user.Password;
            efUser.IsEnabled = user.IsEnabled;
            _AfekaTorrentObjectContext.Users.Attach(efUser);
            _AfekaTorrentObjectContext.SaveChanges();
        }

        public int GetUsersCount()
        {
            return _AfekaTorrentObjectContext.Users.Count();
        }

        public void UpdateFolders(string download, string shared, Guid UserId)
        {
            EF.User efUser = _AfekaTorrentObjectContext.Users.Where(o => o.UserID == UserId).FirstOrDefault();
            efUser.SharedFolder = shared;
            efUser.DownloadFolder = download;
            
           if (efUser.IsActive == true)
                efUser.IsActive = false;
            else
                efUser.IsActive = true;
            _AfekaTorrentObjectContext.Users.Attach(efUser);
            _AfekaTorrentObjectContext.SaveChanges();

        }
    }
}