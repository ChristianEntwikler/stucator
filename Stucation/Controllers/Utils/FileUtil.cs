using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Configuration;
using Stucation.Models;

namespace Stucation.Controllers.Utils
{
    public class FileUtil
    {
        public void WriteToFile(string Message, string repoName, string storeType)
        {
            string repo = WebConfigurationManager.AppSettings["repo"].ToString();
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\" + repo;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\"+repo+"\\" + repoName + ".json";

            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                if (storeType.Equals("APPEND"))
                {
                    using (StreamWriter sw = File.AppendText(filepath))
                    {
                        sw.WriteLine(Message);
                    }
                }
                else if (storeType.Equals("UPDATE"))
                {
                    using (StreamWriter sw = File.CreateText(filepath))
                    {
                        sw.WriteLine(Message);
                    }

                }
            }
        }

        public ResponseDto ReadFromFile(string repoName)

        {
            ResponseDto resp = new ResponseDto();
            string repo = WebConfigurationManager.AppSettings["repo"].ToString();
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\" + repo;

            if (!Directory.Exists(path))
            {
                resp.responseCode = "01";
                resp.responseMessage = "Directory empty or not found";
            }

            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\" + repo + "\\" + repoName + ".json";

            if (!File.Exists(filepath))
            {
                resp.responseCode = "01";
                resp.responseMessage = "File empty or not found";
            }
            else
            {
                resp.responseCode = "00";
                resp.responseMessage = File.ReadAllText(filepath);
            }

            return resp;
        }

        }
}