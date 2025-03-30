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
    public class StudentController : Controller
    {
        // GET: Student

        [HttpGet]
        public ActionResult AddStudent()
        {
            try
            { 
            List<CourseDto> dataList = new List<CourseDto>();
            ResponseDto res = new FileUtil().ReadFromFile("course");
            if (res.responseCode.Equals("00"))
            {
                dataList = JsonConvert.DeserializeObject<List<CourseDto>>(res.responseMessage);
            }

            ViewBag.CourseList = new ListHelper().ToSelectCourseList(dataList);
            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return View();
        }

        public ActionResult EditStudent()
        {
            StudentDto dataObj = new StudentDto();
            try
            { 
            string idHolder = Request.QueryString["i"];
            ResponseDto res = new FileUtil().ReadFromFile("student");
            if (res.responseCode.Equals("00"))
            {
                List<StudentDto> dataList = JsonConvert.DeserializeObject<List<StudentDto>>(res.responseMessage);
                dataObj = dataList.Find(p => p.studentId == idHolder);
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
        public ActionResult ViewStudents()
        {
            List<StudentDto> dataList = new List<StudentDto>();
            try
            { 
            ResponseDto res = new FileUtil().ReadFromFile("student");
            if (res.responseCode.Equals("00"))
            {
                dataList = JsonConvert.DeserializeObject<List<StudentDto>>(res.responseMessage);
            }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return View(dataList);
        }

        public ActionResult StudentProfile()
        {
            StudentDto dataObj = new StudentDto();
            try
            {
            string idHolder = Request.QueryString["i"];
            ResponseDto res = new FileUtil().ReadFromFile("student");
            if (res.responseCode.Equals("00"))
            {
                List<StudentDto> dataList = JsonConvert.DeserializeObject<List<StudentDto>>(res.responseMessage);
                dataObj = dataList.Find(p => p.studentId == idHolder);
            }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return View(dataObj);
        }

        [HttpPost]
        public ActionResult SubmitData(StudentDto model)
        {
            try
            { 
            //if (ModelState.IsValid)
            {
                model.studentId = new ProcessUtil().generateId("STUDENT");
                model.dateCreated = new ProcessUtil().currentPeriod();

                ResponseDto resp = new ResponseDto();
                resp.responseCode = "00";
                resp.responseMessage = "Success";
                List<StudentDto> dataList = new List<StudentDto>();

                ResponseDto res = new FileUtil().ReadFromFile("student");
                if (res.responseCode.Equals("00"))
                {
                    dataList = JsonConvert.DeserializeObject<List<StudentDto>>(res.responseMessage);

                    if (dataList.Count > 0)
                    {
                        if (dataList.Find(p => (p.mobileNumber == model.mobileNumber) && (p.courseId == model.courseId)) != null)
                        {
                            resp.responseCode = "01";
                            resp.responseMessage = "Data already exists";

                                new LogUtil().WriteLog("ERROR: " + resp.responseCode);
                                new LogUtil().WriteLog("ERROR: " + resp.responseMessage + " " + model.mobileNumber + ", " + model.courseId);
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

                    new FileUtil().WriteToFile(new ProcessUtil().convertToJson(dataList), "student", saveMode);

                        ResponseDto res1 = new FileUtil().ReadFromFile("sequence");
                        SequenceListDto seqdata = JsonConvert.DeserializeObject<SequenceListDto>(res1.responseMessage);
                        SequenceDto seq = seqdata.data.FirstOrDefault(p => p.sequenceType == "STUDENT");
                        String idSuffix =new ProcessUtil().sequenceIncrease(seq);
                        new ProcessUtil().updateSequence("STUDENT", idSuffix);
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
        public ActionResult StudentModule()
        {
            List<StudentModuleDto> cmDataList = new List<StudentModuleDto>();
            try
            {
                string idHolder = Request.QueryString["i"];
                StudentDto dataObj = new StudentDto();
                ResponseDto res = new FileUtil().ReadFromFile("student");
                if (res.responseCode.Equals("00"))
                {
                    List<StudentDto> dataLst = JsonConvert.DeserializeObject<List<StudentDto>>(res.responseMessage);
                    dataObj = dataLst.Find(p => p.studentId == idHolder);
                }

                ViewBag.Student = new ListHelper().ToSelectSingleStudentList(dataObj);


                List<ModuleDto> dataList = new List<ModuleDto>();
                res = new FileUtil().ReadFromFile("module");
                if (res.responseCode.Equals("00"))
                {
                    dataList = JsonConvert.DeserializeObject<List<ModuleDto>>(res.responseMessage);
                }

                ViewBag.ModuleList = new ListHelper().ToSelectModuleList(dataList);


                res = new FileUtil().ReadFromFile("studentmodule");
                if (res.responseCode.Equals("00"))
                {
                    List<StudentModuleDto> cmDataList1 = JsonConvert.DeserializeObject<List<StudentModuleDto>>(res.responseMessage);
                    cmDataList = cmDataList1.FindAll(p => p.studentId == idHolder);
                }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            var tupleModel = new Tuple<StudentModuleDto, List<StudentModuleDto>>(new StudentModuleDto(), cmDataList);
            return View(tupleModel);
        }

        [HttpPost]
        public ActionResult SubmitStudentModule([Bind(Prefix = "Item1")] StudentModuleDto model)
        {
            try
            {
                //if (ModelState.IsValid)
                {
                    model.studentModuleId = model.studentId + model.moduleId;
                    model.dateCreated = new ProcessUtil().currentPeriod();

                    ResponseDto resp = new ResponseDto();
                    resp.responseCode = "00";
                    resp.responseMessage = "Success";
                    List<StudentModuleDto> dataList = new List<StudentModuleDto>();

                    ResponseDto res = new FileUtil().ReadFromFile("studentmodule");
                    if (res.responseCode.Equals("00"))
                    {
                        dataList = JsonConvert.DeserializeObject<List<StudentModuleDto>>(res.responseMessage);

                        if (dataList.Count > 0)
                        {
                            if (dataList.Find(p => (p.studentId == model.studentId) && (p.moduleId == model.moduleId)) != null)
                            {
                                resp.responseCode = "01";
                                resp.responseMessage = "Data already exists";

                                new LogUtil().WriteLog("ERROR: " + resp.responseCode);
                                new LogUtil().WriteLog("ERROR: " + resp.responseMessage + " " + model.studentModuleId);
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

                        new FileUtil().WriteToFile(new ProcessUtil().convertToJson(dataList), "studentmodule", saveMode);
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
        public ActionResult StudentAssessment()
        {
            List<StudentAssessmentDto> cmDataList = new List<StudentAssessmentDto>();
            try
            {
                string idHolder = Request.QueryString["i"];
                StudentDto dataObj = new StudentDto();
                ResponseDto res = new FileUtil().ReadFromFile("student");
                if (res.responseCode.Equals("00"))
                {
                    List<StudentDto> dataLst = JsonConvert.DeserializeObject<List<StudentDto>>(res.responseMessage);
                    dataObj = dataLst.Find(p => p.studentId == idHolder);
                }

                ViewBag.Student = new ListHelper().ToSelectSingleStudentList(dataObj);


                List<AssessmentDto> dataList = new List<AssessmentDto>();
                res = new FileUtil().ReadFromFile("assessment");
                if (res.responseCode.Equals("00"))
                {
                    dataList = JsonConvert.DeserializeObject<List<AssessmentDto>>(res.responseMessage);
                }

                ViewBag.AssessmentList = new ListHelper().ToSelectAssessmentList(dataList);


                res = new FileUtil().ReadFromFile("studentassessment");
                if (res.responseCode.Equals("00"))
                {
                    List<StudentAssessmentDto> cmDataList1 = JsonConvert.DeserializeObject<List<StudentAssessmentDto>>(res.responseMessage);
                    cmDataList = cmDataList1.FindAll(p => p.studentId == idHolder);
                }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            var tupleModel = new Tuple<StudentAssessmentDto, List<StudentAssessmentDto>>(new StudentAssessmentDto(), cmDataList);
            return View(tupleModel);
        }

        [HttpPost]
        public ActionResult SubmitStudentAssessment([Bind(Prefix = "Item1")] StudentAssessmentDto model)
        {
            try
            {
                //if (ModelState.IsValid)
                {
                    model.studentAssessmentId = model.studentId + model.AssessmentId;
                    model.dateCreated = new ProcessUtil().currentPeriod();

                    ResponseDto resp = new ResponseDto();
                    resp.responseCode = "00";
                    resp.responseMessage = "Success";
                    List<StudentAssessmentDto> dataList = new List<StudentAssessmentDto>();

                    ResponseDto res = new FileUtil().ReadFromFile("studentassessment");
                    if (res.responseCode.Equals("00"))
                    {
                        dataList = JsonConvert.DeserializeObject<List<StudentAssessmentDto>>(res.responseMessage);

                        if (dataList.Count > 0)
                        {
                            if (dataList.Find(p => (p.studentId == model.studentId) && (p.AssessmentId == model.AssessmentId)) != null)
                            {
                                resp.responseCode = "01";
                                resp.responseMessage = "Data already exists";

                                new LogUtil().WriteLog("ERROR: " + resp.responseCode);
                                new LogUtil().WriteLog("ERROR: " + resp.responseMessage + " " + model.studentAssessmentId);
                            }

                        }
                    }
                    res = new FileUtil().ReadFromFile("assessment");
                    AssessmentDto dt = new AssessmentDto();
                    if (res.responseCode.Equals("00"))
                    {
                        List<AssessmentDto> dataLst = JsonConvert.DeserializeObject<List<AssessmentDto>>(res.responseMessage);
                        dt = dataLst.FirstOrDefault(p => p.assessmentId == model.AssessmentId);

                    }

                    if (int.Parse(model.score) <= dt.assessmentTotalScore)
                    {
                        if (resp.responseCode.Equals("00"))
                        {
                            string saveMode = "UPDATE";
                            if (dataList.Count < 1)
                            {
                                saveMode = "APPEND";
                            }
                            dataList.Add(model);

                            new FileUtil().WriteToFile(new ProcessUtil().convertToJson(dataList), "studentassessment", saveMode);
                        }
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


        public ActionResult ViewStudentModules()
        {
            List<ViewStudentModuleDto> dataList = new List<ViewStudentModuleDto>();
            List<StudentModuleDto> dataList1 = new List<StudentModuleDto>();
            try
            {
                string idHolder = WebConfigurationManager.AppSettings["adminUser"].ToString();
                
                ResponseDto res = new FileUtil().ReadFromFile("studentmodule");
                if (res.responseCode.Equals("00"))
                {
                    dataList1 = JsonConvert.DeserializeObject<List<StudentModuleDto>>(res.responseMessage);

                    if (dataList1 != null)
                    {
                        foreach (StudentModuleDto dt in dataList1)
                        {
                            ViewStudentModuleDto sm = new ViewStudentModuleDto();
                            sm.studentModule = dt;
                            sm.student = new ProcessUtil().fetchStudentDetails(dt.studentId);
                            sm.module= new ProcessUtil().fetchModuleDetails(dt.moduleId);
                            dataList.Add(sm);
                        }
                    }
                }

                if(idHolder == WebConfigurationManager.AppSettings["adminUser"].ToString())
                {
                    dataList = dataList;
                }
                else
                {
                    dataList = dataList.FindAll(p=>p.studentModule.studentId == idHolder);
                }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return View(dataList);
        }
        
        public ActionResult ViewStudentAssessments()
        {
            List<ViewStudentAssessmentDto> dataList = new List<ViewStudentAssessmentDto>();
            List<StudentAssessmentDto> dataList1 = new List<StudentAssessmentDto>();
            try
            {
                string idHolder = WebConfigurationManager.AppSettings["adminUser"].ToString();
                
                ResponseDto res = new FileUtil().ReadFromFile("studentassessment");
                if (res.responseCode.Equals("00"))
                {
                    dataList1 = JsonConvert.DeserializeObject<List<StudentAssessmentDto>>(res.responseMessage);

                    if (dataList1 != null)
                    {
                        foreach (StudentAssessmentDto dt in dataList1)
                        {
                            ViewStudentAssessmentDto sm = new ViewStudentAssessmentDto();
                            sm.studentAssessment = dt;
                            sm.student = new ProcessUtil().fetchStudentDetails(dt.studentId);
                            sm.assessment = new ProcessUtil().fetchAssessmentDetails(dt.AssessmentId);
                            dataList.Add(sm);
                        }
                    }
                }

                if(idHolder == WebConfigurationManager.AppSettings["adminUser"].ToString())
                {
                    dataList = dataList;
                }
                else
                {
                    dataList = dataList.FindAll(p=>p.studentAssessment.studentId == idHolder);
                }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return View(dataList);
        }
        public ActionResult ViewStudentReport()
        {
            List<ViewStudentReportDto> dataList2 = new List<ViewStudentReportDto>();
            List<ViewStudentAssessmentDto> dataList = new List<ViewStudentAssessmentDto>();
            List<StudentAssessmentDto> dataList1 = new List<StudentAssessmentDto>();
            try
            {
                string idHolder = WebConfigurationManager.AppSettings["adminUser"].ToString();
                
                ResponseDto res = new FileUtil().ReadFromFile("studentassessment");
                if (res.responseCode.Equals("00"))
                {
                    dataList1 = JsonConvert.DeserializeObject<List<StudentAssessmentDto>>(res.responseMessage);

                    if (dataList1 != null)
                    {
                        foreach (StudentAssessmentDto dt in dataList1)
                        {
                            ViewStudentAssessmentDto sm = new ViewStudentAssessmentDto();
                            sm.studentAssessment = dt;
                            sm.student = new ProcessUtil().fetchStudentDetails(dt.studentId);
                            sm.assessment = new ProcessUtil().fetchAssessmentDetails(dt.AssessmentId);
                            sm.module = new ProcessUtil().fetchModuleDetails(sm.assessment.moduleId);
                            dataList.Add(sm);
                        }

                        List<ViewStudentAssessmentDto> dtList = dataList.OrderBy(p=>p.student.studentId).ToList();

                        String modId = "";
                        foreach(ViewStudentAssessmentDto dt in dtList)
                        {
                            if (modId.Equals(""))
                            {
                                modId = dt.assessment.moduleId;
                            }
                            if (modId.Equals(dt.module.moduleId))
                            {
       
                            }
                            else
                            {
                                modId = dt.assessment.moduleId;
                            }

                            ViewStudentReportDto vsr = new ViewStudentReportDto();
                            vsr.viewStudentAssessment = dt;
                            vsr.totalScore = vsr.totalScore + float.Parse(dt.studentAssessment.score);
                        }
                    }
                }

                if(idHolder == WebConfigurationManager.AppSettings["adminUser"].ToString())
                {
                    dataList2 = dataList2;
                }
                else
                {
                    dataList2 = dataList2.FindAll(p=>p.viewStudentAssessment.studentAssessment.studentId == idHolder);
                }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return View(dataList2);
        }


    }
}