using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

//Ændre til jeres namespace
namespace SystemIoTasks.Controllers
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



            //Gem billedet og vis det i viewet
            return View();
        }

        //Omdanner vores array til et liste af MonthAndValue
        public static List<MonthAndValue> GetValuesFromArray(int[] array)
        {
            List<MonthAndValue> values = new List<MonthAndValue>();
            int i = 1;
            foreach (int value in array)
            {
                values.Add(new MonthAndValue()
                {
                    MonthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i),
                    YValue = value
                });
                i++;
            }


            return values;
        }
    }

    
    public class MonthAndValue
    {
        public int YValue { get; set; }
        public string MonthName { get; set; }
    }
}