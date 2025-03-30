using Newtonsoft.Json;
using Stucation.Controllers.Utils;
using Stucation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Stucation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            CounterDto counter = new CounterDto();
            try
            {


                ResponseDto res = new FileUtil().ReadFromFile("course");
            if (res.responseCode.Equals("00"))
            {
                List<CourseDto> dataList = JsonConvert.DeserializeObject<List<CourseDto>>(res.responseMessage);
                counter.courseCounter = dataList.Count;
            }



            res = new FileUtil().ReadFromFile("module");
            if (res.responseCode.Equals("00"))
            {
                List<ModuleDto> dataList = JsonConvert.DeserializeObject<List<ModuleDto>>(res.responseMessage);
                counter.moduleCounter = dataList.Count;
            }

                 res = new FileUtil().ReadFromFile("student");
                if (res.responseCode.Equals("00"))
                {
                    List<StudentDto> dataList = JsonConvert.DeserializeObject<List<StudentDto>>(res.responseMessage);
                    counter.studentCounter = dataList.Count;
                }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return View("index", counter);
        }
        
    }
}