using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SMSapplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new eHedwig());
        }

       public static string[] PhoneNumberList;

       static bool list = false;
       public static bool isListSelected
       {
           get
           {
               return list;

           }
           set
           {
               list = value;
           }
       }
    }
}