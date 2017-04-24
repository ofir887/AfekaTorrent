using Administration.Models;
using Entities;
using AfekaTorrent.DownloadManager.UserServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AfekaTorrent.DownloadManager.FileServer;

namespace Administration.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult RegisterUser(RegisterViewModel model)
        {
            UserServiceClient Usc = new UserServiceClient();
            
            if (ModelState.IsValid)
            {
                var user = new Entities.User { UserName = model.UserName, Password = model.Password, DownloadFolder ="", SharedFolder = "",IsEnabled= model.IsEnabled ,IsActive=false };
                Usc.AddUser(user);
                return RedirectToAction("Users", "User");

            }

            
            return View(model);
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Users()
        {
            UserServiceClient Usc = new UserServiceClient();

            ViewBag.UsersCount = Usc.GetUsersCount(); //view number of users
           
            

            List<Entities.User> userList = new List<Entities.User>();
            List<User> userModel = new List<User>();
            foreach (Entities.User user in Usc.GetAllUsers())
            {
                Entities.User element = new Entities.User();
                element.UserName = user.UserName;
                element.UserID = user.UserID;
                element.Password = user.Password;
                element.IsEnabled = user.IsEnabled;
                element.IsActive = user.IsActive; //
                element.SharedFolder = user.SharedFolder;
                element.DownloadFolder = user.DownloadFolder;
                userList.Add(element);
            }
            
            return View(userList);
        }

        public ActionResult DeleteUser(string UserID)
        {
            UserServiceClient Usc = new UserServiceClient();
            Usc.DeleteUser(Guid.Parse(UserID));

            return RedirectToAction("Users", "User");
        }
        public ActionResult DisplayUser(Guid UserID)
        {
            UserServiceClient Usc = new UserServiceClient();
            User user = Usc.GetUser(UserID);
            RegisterViewModel displayUserModel = new RegisterViewModel();
            displayUserModel.UserName = user.UserName;
            displayUserModel.UserID = UserID;
            displayUserModel.IsEnabled = user.IsEnabled;
            return View(displayUserModel);
        }

        public ActionResult EditUser(RegisterViewModel model)
        {
            UserServiceClient Usc = new UserServiceClient();
            if (ModelState.IsValid)
            {
                var user = new Entities.User {UserID= model.UserID, UserName = model.UserName, Password = model.Password, DownloadFolder = "", SharedFolder = "",IsEnabled = model.IsEnabled };
                Usc.EditUser(user);
                return RedirectToAction("Users", "User");
            }
            return RedirectToAction("DisplayUser", "User",model);
        }
    }
}