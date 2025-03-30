using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Stucation.Controllers.Utils
{
    public class LogUtil
    {
        public void WriteLog(string Message)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";

            if (!Directory.Exists(path))
            {

                Directory.CreateDirectory(path);

            }

            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ApiLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";

            if (!File.Exists(filepath))
            {

                // Create a file to write to.   

                using (StreamWriter sw = File.CreateText(filepath))
                {

                    sw.WriteLine(Message + "............................." + DateTime.Now);

                }

            }
            else
            {

                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message + "............................." + DateTime.Now);

                }

            }

        }

    }
}