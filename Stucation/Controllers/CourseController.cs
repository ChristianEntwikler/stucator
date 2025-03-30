using Newtonsoft.Json;
using Stucation.Controllers.Utils;
using Stucation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Stucation.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult AddCourse()
        {
            return View();
        }
        public ActionResult EditCourse()
        {
            CourseDto dataObj = new CourseDto();
            try
            { 
            string idHolder = Request.QueryString["i"];
            ResponseDto res = new FileUtil().ReadFromFile("course");
            if (res.responseCode.Equals("00"))
            {
                List<CourseDto> dataList = JsonConvert.DeserializeObject<List<CourseDto>>(res.responseMessage);
                dataObj = dataList.Find(p => p.courseId == idHolder);
            }
            if (dataObj == null)
            {
                Response.Redirect("~/Home/index");
            }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
            }
            return View(dataObj);
        }
        public ActionResult ViewCourses()
        {
            List<CourseDto> dataList = new List<CourseDto>();
            try
            { 
            ResponseDto res = new FileUtil().ReadFromFile("course");
            if (res.responseCode.Equals("00"))
            {
                dataList = JsonConvert.DeserializeObject<List<CourseDto>>(res.responseMessage);
            }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
            }
            return View(dataList);
        }
        public ActionResult AboutCourse()
        {
            CourseDto dataObj = new CourseDto();
            try
            { 
            string idHolder = Request.QueryString["i"];
            ResponseDto res = new FileUtil().ReadFromFile("course");
            if (res.responseCode.Equals("00"))
            {
                List<CourseDto> dataList = JsonConvert.DeserializeObject<List<CourseDto>>(res.responseMessage);
                dataObj = dataList.Find(p=>p.courseId==idHolder);
            }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
            }
            return View(dataObj);
        }

        [HttpPost]
        public ActionResult SubmitData(CourseDto model)
        {
            try
            { 
            //if (ModelState.IsValid)
            {
                model.courseId = new ProcessUtil().generateId("COURSE");
                model.dateCreated = new ProcessUtil().currentPeriod();

                ResponseDto resp = new ResponseDto();
                resp.responseCode = "00";
                resp.responseMessage = "Success";
                List<CourseDto> dataList = new List<CourseDto>();

                ResponseDto res = new FileUtil().ReadFromFile("course");
                if (res.responseCode.Equals("00"))
                {
                    dataList = JsonConvert.DeserializeObject<List<CourseDto>>(res.responseMessage);

                    if (dataList.Count > 0)
                    {
                        if(dataList.Find(p => (p.courseName == model.courseName) && (p.coursePeriod == model.coursePeriod)) != null)
                        {
                            resp.responseCode = "01";
                            resp.responseMessage = "Data already exists";

                                new LogUtil().WriteLog("ERROR: " + resp.responseCode);
                                new LogUtil().WriteLog("ERROR: " + resp.responseMessage + " " + model.courseName);
                            }

                    }
                }

                if (resp.responseCode.Equals("00"))
                {
                    string saveMode = "UPDATE";
                    if(dataList.Count < 1)
                    {
                        saveMode = "APPEND";
                    }
                    dataList.Add(model);

                    new FileUtil().WriteToFile(new ProcessUtil().convertToJson(dataList), "course", saveMode);
                    new ProcessUtil().updateSequence("COURSE", model.courseId);
                }
            }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }

            Response.Redirect("~/Home/index");
            return View("~/Home/index");
        }


        [HttpGet]
        public ActionResult CourseModule()
        {
            List<CourseModuleDto> cmDataList = new List<CourseModuleDto>();
            try
            {
                string idHolder = Request.QueryString["i"];
                CourseDto dataObj = new CourseDto();
                ResponseDto res = new FileUtil().ReadFromFile("course");
                if (res.responseCode.Equals("00"))
                {
                    List<CourseDto> dataLst = JsonConvert.DeserializeObject<List<CourseDto>>(res.responseMessage);
                    dataObj = dataLst.Find(p => p.courseId == idHolder);
                }

                ViewBag.Course = new ListHelper().ToSelectSingleCourseList(dataObj);


                List<ModuleDto> dataList = new List<ModuleDto>();
                res = new FileUtil().ReadFromFile("module");
                if (res.responseCode.Equals("00"))
                {
                    dataList = JsonConvert.DeserializeObject<List<ModuleDto>>(res.responseMessage);
                }

                ViewBag.ModuleList = new ListHelper().ToSelectModuleList(dataList);  


                 res = new FileUtil().ReadFromFile("coursemodule");
                if (res.responseCode.Equals("00"))
                {
                    List<CourseModuleDto> cmDataList1 = JsonConvert.DeserializeObject<List<CourseModuleDto>>(res.responseMessage);
                    cmDataList = cmDataList1.FindAll(p => p.courseId == idHolder);
                }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            var tupleModel = new Tuple<CourseModuleDto, List<CourseModuleDto>>(new CourseModuleDto(), cmDataList);
            return View(tupleModel);
        }

        [HttpPost]
        public ActionResult SubmitCourseModule([Bind(Prefix = "Item1")] CourseModuleDto model)
        {
            try
            {
                //if (ModelState.IsValid)
                {
                    model.courseModuleId = model.courseId + model.moduleId;
                    model.dateCreated = new ProcessUtil().currentPeriod();

                    ResponseDto resp = new ResponseDto();
                    resp.responseCode = "00";
                    resp.responseMessage = "Success";
                    List<CourseModuleDto> dataList = new List<CourseModuleDto>();

                    ResponseDto res = new FileUtil().ReadFromFile("coursemodule");
                    if (res.responseCode.Equals("00"))
                    {
                        dataList = JsonConvert.DeserializeObject<List<CourseModuleDto>>(res.responseMessage);

                        if (dataList.Count > 0)
                        {
                            if (dataList.Find(p => (p.courseId == model.courseId) && (p.moduleId == model.moduleId)) != null)
                            {
                                resp.responseCode = "01";
                                resp.responseMessage = "Data already exists";

                                new LogUtil().WriteLog("ERROR: " + resp.responseCode);
                                new LogUtil().WriteLog("ERROR: " + resp.responseMessage + " " + model.courseModuleId);
                            }

                        }
                    }

                    if (resp.responseCode.Equals("00"))
                    {
                        string saveMode = "UPDATE";
                        if (dataList.Count < 1)
                        {
                            saveMode = "APPEND";
                        }
                        dataList.Add(model);

                        new FileUtil().WriteToFile(new ProcessUtil().convertToJson(dataList), "coursemodule", saveMode);
                    }
                }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            Response.Redirect("~/Home/index");
            return View("~/Home/index");
        }

        public ActionResult ViewCourseModules()
        {
            List<ViewCourseModuleDto> dataList = new List<ViewCourseModuleDto>();
            List<CourseModuleDto> dataList1 = new List<CourseModuleDto>();
            try
            {
                string idHolder = Request.QueryString["i"];

                ResponseDto res = new FileUtil().ReadFromFile("coursemodule");
                if (res.responseCode.Equals("00"))
                {
                    dataList1 = JsonConvert.DeserializeObject<List<CourseModuleDto>>(res.responseMessage);

                    if (dataList1 != null)
                    {
                        foreach (CourseModuleDto dt in dataList1)
                        {
                            ViewCourseModuleDto sm = new ViewCourseModuleDto();
                            sm.courseModule = dt;
                            sm.course = new ProcessUtil().fetchCourseDetails(dt.courseId);
                            sm.module = new ProcessUtil().fetchModuleDetails(dt.moduleId);
                            dataList.Add(sm);
                        }
                    }
                }

                if (idHolder == WebConfigurationManager.AppSettings["adminUser"].ToString())
                {
                    dataList = dataList;
                }
                else
                {
                    dataList = dataList.FindAll(p => p.courseModule.courseId == idHolder);
                }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return View(dataList);
        }


    }
}