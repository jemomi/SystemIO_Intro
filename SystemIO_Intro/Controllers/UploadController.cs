using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemIO_Intro.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(List<HttpPostedFileBase> files, string album)
        {
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    //Der er en fil

                    string path = Server.MapPath("~/Content/Upload/" + album + "/");

                    if (!Directory.Exists(path))
                    {
                        //hvis mappe/album ikke eksistere bliver der oprettet
                        Directory.CreateDirectory(path);
                    }

                    file.SaveAs(path + file.FileName);
                }
            }
            return View();
        }
    }
}