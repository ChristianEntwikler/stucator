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
    public class AssessmentController : Controller
    {
        // GET: Assessment

        [HttpGet]
        public ActionResult AddAssessment()
        {
            try
            {
                List<ModuleDto> dataList = new List<ModuleDto>();
                ResponseDto res = new FileUtil().ReadFromFile("module");
                if (res.responseCode.Equals("00"))
                {
                    dataList = JsonConvert.DeserializeObject<List<ModuleDto>>(res.responseMessage);
                }

                ViewBag.ModuleList = new ListHelper().ToSelectModuleList(dataList);
            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return View();
        }

        public ActionResult EditAssessment()
        {
            AssessmentDto dataObj = new AssessmentDto();
            try
            {
                string idHolder = Request.QueryString["i"];
                ResponseDto res = new FileUtil().ReadFromFile("assessment");
                if (res.responseCode.Equals("00"))
                {
                    List<AssessmentDto> dataList = JsonConvert.DeserializeObject<List<AssessmentDto>>(res.responseMessage);
                    dataObj = dataList.Find(p => p.assessmentId == idHolder);
                }
                if (dataObj == null)
                {
                    Response.Redirect("~/Home/index");
                }

            }
            catch(Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return View(dataObj);
        }
        public ActionResult ViewAssessments()
        {
            List<AssessmentDto> dataList = new List<AssessmentDto>();
            try
            { 
            ResponseDto res = new FileUtil().ReadFromFile("assessment");
            if (res.responseCode.Equals("00"))
            {
                dataList = JsonConvert.DeserializeObject<List<AssessmentDto>>(res.responseMessage);
            }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return View(dataList);
        }
        public ActionResult StudentAssessment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitData(AssessmentDto model)
        {
            try
            { 
            //if (ModelState.IsValid)
            {
                model.assessmentId = new ProcessUtil().generateId("ASSESSMENT");
                model.dateCreated = new ProcessUtil().currentPeriod();

                ResponseDto resp = new ResponseDto();
                resp.responseCode = "00";
                resp.responseMessage = "Success";
                List<AssessmentDto> dataList = new List<AssessmentDto>();

                ResponseDto res = new FileUtil().ReadFromFile("assessment");
                if (res.responseCode.Equals("00"))
                {
                    dataList = JsonConvert.DeserializeObject<List<AssessmentDto>>(res.responseMessage);

                    if (dataList.Count > 0)
                    {
                        if (dataList.Find(p => (p.assessmentTitle == model.assessmentTitle)) != null)
                        {
                            resp.responseCode = "01";
                            resp.responseMessage = "Data already exists";

                                new LogUtil().WriteLog("ERROR: " + resp.responseCode);
                                new LogUtil().WriteLog("ERROR: " + resp.responseMessage + " " + model.assessmentTitle);
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

                    new FileUtil().WriteToFile(new ProcessUtil().convertToJson(dataList), "assessment", saveMode);
                    new ProcessUtil().updateSequence("ASSESSMENT", model.moduleId);
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

    }
}