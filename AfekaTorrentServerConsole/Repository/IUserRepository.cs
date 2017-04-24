using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AfekaTorrentServerConsole.Repository
{
    public interface IUserRepository
    {
        void AddUser(AfekaTorrentServerConsole.EF.User user);
        List<EF.User> GetAllUsers();

        void DeleteUser(Guid UserID);

        EF.User GetUser(Guid UserID);

        void EditUser(User user);

        int GetUsersCount();

        Guid Login(string userName, string password);

        void UpdateFolders(string download, string shared, Guid UserId);
    }
}