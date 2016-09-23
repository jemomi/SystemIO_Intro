using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

//Ændre til jeres namespace
namespace SystemIO_Intro.Controllers
{
    public class BarchartController : Controller
    {
        // GET: Barchart
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int[] xValues)
        {
            //Laver vores array til et MonthAndValue List Class
            List<MonthAndValue> values = GetValuesFromArray(xValues);

            //Brug values til at lav et bar chart billede.
            int spaceing = 20;
            int intBottomBarHeight = 30;
            int intLeftContentWidth = 75;
            int intTopSpace = 30;
            int maxHeight = 0;
            foreach (var month in values)
            {
                maxHeight = month.YValue > maxHeight ? month.YValue : maxHeight;
            }

            Bitmap bmpChart = new Bitmap(1024, maxHeight + intBottomBarHeight + intTopSpace);
            Graphics gfxChart = Graphics.FromImage(bmpChart);
            gfxChart.FillRectangle(new SolidBrush(Color.White), 0, 0, bmpChart.Width, bmpChart.Height);
            for (int i = 0; i <= 10; i++)
            {
                decimal decorInterval = (decimal)(maxHeight / Convert.ToDecimal(10) * (Convert.ToDecimal(10) - Convert.ToDecimal(i)));
                int decorY = maxHeight / 10 * i + intTopSpace;
                Pen penMonthVal = new Pen(new SolidBrush(Color.Black), 1);
                gfxChart.DrawLine(penMonthVal, 0, decorY, bmpChart.Width, decorY);

                gfxChart.DrawString(Math.Round(decorInterval).ToString(), new Font("Arial", 15), new SolidBrush(Color.Black), 0, decorY);
            }
            for (int i = 0; i < values.Count; i++)
            {
                Color bgColor = values[i].bgColor;
                int intMonthBarWidth = ((bmpChart.Width - intLeftContentWidth) / values.Count) - spaceing - (spaceing / values.Count);
                Rectangle rectMonthBar = new Rectangle((spaceing * (i + 1)) + (intMonthBarWidth * i) + intLeftContentWidth, maxHeight - values[i].YValue + intTopSpace, intMonthBarWidth, values[i].YValue);
                Brush brushMonthBar = new SolidBrush(bgColor);
                Pen penMonthVal = new Pen(brushMonthBar, 2);
                gfxChart.DrawLine(penMonthVal, 0, rectMonthBar.Y + (penMonthVal.Width / 2), rectMonthBar.X + rectMonthBar.Width, rectMonthBar.Y + (penMonthVal.Width / 2));
            }
            for (int i = 0; i < values.Count; i++)
            {
                int intMonthBarWidth = ((bmpChart.Width - intLeftContentWidth) / values.Count) - spaceing - (spaceing / values.Count);

                Color bgColor = Color.FromArgb(235, values[i].bgColor);
                Rectangle rectMonthBar = new Rectangle((spaceing * (i + 1)) + (intMonthBarWidth * i) + intLeftContentWidth, maxHeight - values[i].YValue + intTopSpace, intMonthBarWidth, values[i].YValue);
                Brush brushMonthBar = new SolidBrush(bgColor);

                gfxChart.FillRectangle(brushMonthBar, rectMonthBar);

                Font fontMonth = new Font("Arial", 10);
                StringFormat sf = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                int bgAvg = (bgColor.R + bgColor.G + bgColor.B) / 3;

                Brush brushMonthVal = new SolidBrush(bgAvg > 100 ? Color.Black : Color.White);
                Rectangle rectMonthVal = new Rectangle(rectMonthBar.X, rectMonthBar.Y, rectMonthBar.Width, 40);

                gfxChart.DrawString(values[i].YValue.ToString(), fontMonth, brushMonthVal, rectMonthVal, sf);

                Rectangle rectMonthName = new Rectangle(rectMonthBar.X, rectMonthBar.Y + rectMonthBar.Height, intMonthBarWidth, intBottomBarHeight);

                gfxChart.DrawString(values[i].MonthName.Substring(0, 3), fontMonth, new SolidBrush(Color.Black), rectMonthName, sf);
            }

            MemoryStream ms = new MemoryStream();
            bmpChart.Save(ms, ImageFormat.Jpeg);

            bmpChart.Save(Server.MapPath("~/Content/Upload/Barcharts/" + Guid.NewGuid() + ".jpg"), ImageFormat.Jpeg);

            //Gem billedet og vis det i viewet
            return View(ms);
        }

        //Omdanner vores array til et liste af MonthAndValue
        public static List<MonthAndValue> GetValuesFromArray(int[] array)
        {
            Random rnd = new Random();

            List<MonthAndValue> values = new List<MonthAndValue>();
            int i = 1;
            foreach (int value in array)
            {
                if (value != 0)
                {
                    values.Add(new MonthAndValue()
                    {
                        MonthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i),
                        YValue = value,
                        bgColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256))
                    });
                }
                i++;
            }


            return values;
        }
    }


    public class MonthAndValue
    {
        public int YValue { get; set; }
        public string MonthName { get; set; }
        public Color bgColor { get; set; }
    }
}