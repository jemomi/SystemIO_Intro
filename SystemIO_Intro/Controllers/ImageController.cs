using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemIO_Intro.Controllers
{
    public class ImageController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, int maxHeight, int maxWidth)
        {
            if (file != null && file.ContentLength > 0)
            {
                Image img = Image.FromStream(file.InputStream);
                //Graphics gfx = Graphics.FromImage(img);

                Image resizedImg = ResizeImage(img, maxHeight, maxWidth);

                MemoryStream ms = new MemoryStream();

                resizedImg.Save(ms, ImageFormat.Jpeg);

                return View(ms);
            }
            return View();
        }

        public static Image ResizeImage(Image img, int Height, int Width)
        {
            double ratioX = (double)Width / img.Width;
            double ratioY = (double)Height / img.Height;

            double ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(img.Width * ratio);
            int newHeight = (int)(img.Height * ratio);

            Bitmap bitImg = new Bitmap(newWidth, newHeight);
            Graphics gfxImg = Graphics.FromImage(bitImg);

            gfxImg.DrawImage(img, 0, 0, newWidth, newHeight);

            return bitImg;
        }
    }
}