using AfekaTorrent.DownloadManager.FileServer;
using AfekaTorrent.DownloadManager.UserServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administration.Controllers
{
    public class FilesController : Controller
    {
        // GET: Files
        public ActionResult Files(string SearchString)
        {
            
            
            FilesServiceClient fsc = new FilesServiceClient();


            List<Entities.File> fileList = new List<Entities.File>();

            Entities.File[] files = fsc.GetAllFiles();

            ViewBag.FilesCount = files.Count();


            if (SearchString==null || SearchString.Equals(""))
            {
                

                return View(files.ToList());
            }
           
            else
            {
                 fileList = files.ToList().Where(x=>x.FileName.ToLower().Contains(SearchString.ToLower())).ToList();
                return View(fileList);
            }
        }
    }
}