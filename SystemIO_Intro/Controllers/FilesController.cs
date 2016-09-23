using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemIO_Intro.Controllers
{
    public class FilesController : Controller
    {
        // GET: Files
        public ActionResult Index()
        {
            List<String> files = Directory.GetFiles(Server.MapPath("~/Content/Upload/winDefImgs")).ToList();

            List<FileInfo> listFileInfo = new List<FileInfo>();
            foreach (string file in files)
            {
                listFileInfo.Add(new FileInfo(file));
            }

            return View(listFileInfo);
        }

        public ActionResult Delete(string name)
        {
            string path = Server.MapPath("~/Content/Upload/winDefImgs/");
            FileInfo file = new FileInfo(path + name);
            file.Delete();
            
            return RedirectToAction("Index");
        }
    }
}