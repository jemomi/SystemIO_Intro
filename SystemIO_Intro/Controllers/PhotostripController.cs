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
    public class PhotostripController : Controller
    {
        // GET: Photostrip
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(List<HttpPostedFileBase> files, int maxWidth, string authorName, bool addFilmDecor, bool addImageNameDecor, bool addCopyrightDecor)
        {
            int padding = 20;
            int stripHeight = 0;
            List<Image> listImgs = new List<Image>();
            //List<MemoryStream> listMS = new List<MemoryStream>();
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    Image img = Image.FromStream(file.InputStream);

                    Image resizedImg = ResizeImage(img, maxWidth, (padding / 2));

                    if (addImageNameDecor)
                    {
                        Graphics gfxResImg = Graphics.FromImage(resizedImg);

                        SolidBrush stripImgBrush = new SolidBrush(Color.FromArgb(125, Color.Black));
                        Rectangle stripImgRect = new Rectangle(0, 0, resizedImg.Width, 30);
                        stripImgRect.X = resizedImg.Width - stripImgRect.Width;
                        stripImgRect.Y = resizedImg.Height - stripImgRect.Height;
                        gfxResImg.FillRectangle(stripImgBrush, stripImgRect);

                        Font stripImgFont = new Font("Arial", 8);
                        StringFormat stripImgSF = new StringFormat()
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };
                        stripImgBrush.Color = Color.White;
                        gfxResImg.DrawString(file.FileName, stripImgFont, stripImgBrush, (resizedImg.Width / 2), (stripImgRect.Y + stripImgRect.Height / 2), stripImgSF);
                    }

                    stripHeight += resizedImg.Height;

                    listImgs.Add(resizedImg);

                    //MemoryStream ms = new MemoryStream();
                    //resizedImg.Save(ms, ImageFormat.Jpeg);
                    //listMS.Add(ms);
                }
            }
            int decorFilmSize = 0;
            if (addFilmDecor)
            {
                decorFilmSize = maxWidth / 8;
            }

            int copyrightHeight = padding / 2;
            string strCopyright = "Lavet af: " + authorName;
            Font myFont = new Font("Arial", 12);
            if (addCopyrightDecor)
            {
                Graphics gfxCopyright = Graphics.FromImage(new Bitmap(maxWidth, stripHeight));
                copyrightHeight = (int)gfxCopyright.MeasureString(strCopyright, myFont, maxWidth).Height + (padding * 2);

                //copyrightHeight = 45;
            }

            Bitmap photoStrip = new Bitmap(maxWidth + (decorFilmSize * 2), stripHeight + copyrightHeight);
            Graphics gfxStrip = Graphics.FromImage(photoStrip);
            int offsetImg = 0;
            foreach (Image img in listImgs)
            {
                gfxStrip.DrawImage(img, decorFilmSize, offsetImg, img.Width, img.Height);

                offsetImg += img.Height;
            }


            SolidBrush myBrush = new SolidBrush(Color.FromArgb(125, Color.Black));

            if (addCopyrightDecor)
            {
                Rectangle myRect = new Rectangle(0, 0, photoStrip.Width, copyrightHeight);
                myRect.X = photoStrip.Width - myRect.Width;
                myRect.Y = photoStrip.Height - myRect.Height;
                gfxStrip.FillRectangle(myBrush, myRect);

                StringFormat sf = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                myBrush.Color = Color.White;
                gfxStrip.DrawString(strCopyright, myFont, myBrush, myRect, sf);
            }

            if (addFilmDecor)
            {
                int decorFilmAmount = stripHeight / decorFilmSize;
                for (int i = 0; i < decorFilmAmount; i++)
                {
                    int decorFilmOffsetY = decorFilmSize * i;

                    myBrush.Color = Color.White;
                    gfxStrip.FillRectangle(myBrush, 0, decorFilmOffsetY, decorFilmSize, decorFilmSize);
                    gfxStrip.FillRectangle(myBrush, (photoStrip.Width - decorFilmSize), decorFilmOffsetY, decorFilmSize, decorFilmSize);

                    Pen seanpenn = new Pen(Color.Black, (decorFilmSize / 3));
                    gfxStrip.DrawRectangle(seanpenn, 0, decorFilmOffsetY, decorFilmSize, decorFilmSize);
                    gfxStrip.DrawRectangle(seanpenn, (photoStrip.Width - decorFilmSize), decorFilmOffsetY, decorFilmSize, decorFilmSize);
                }
            }

            MemoryStream ms = new MemoryStream();
            photoStrip.Save(ms, ImageFormat.Jpeg);

            photoStrip.Save(Server.MapPath("~/Content/Upload/Photostrips/" + Guid.NewGuid() + ".jpg"), ImageFormat.Jpeg);

            return View(ms);
            //return View(listMS);
        }

        public static Image ResizeImage(Image img, int Width, int padding)
        {
            double ratio = (double)Width / img.Width;

            int newWidth = (int)((img.Width * ratio) - padding);
            int newHeight = (int)((img.Height * ratio) - padding);

            Bitmap bitImg = new Bitmap(newWidth, newHeight);
            Graphics gfxImg = Graphics.FromImage(bitImg);

            gfxImg.DrawImage(img, padding, padding, newWidth - padding, newHeight - padding);

            return bitImg;
        }
    }
}