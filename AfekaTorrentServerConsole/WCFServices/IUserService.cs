﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AfekaTorrentServerConsole.WCFServices
{
    public interface IUserService
    {
        void AddUser(Entities.User user);

        Guid Login(string userName, string password);

        List<Entities.User> GetAllUsers();

        void DeleteUser(Guid UserID);

        User GetUser(Guid UserID);
        void EditUser(Entities.User user);

        int GetUsersCount();

        void UpdateFolders(string download, string shared, Guid UserId);
    }
}