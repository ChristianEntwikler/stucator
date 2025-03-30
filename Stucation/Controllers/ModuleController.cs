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
    public class ModuleController : Controller
    {
        // GET: Module
        public ActionResult AddModule()
        {
            return View();
        }
        public ActionResult EditModule()
        {
            ModuleDto dataObj = new ModuleDto();
            try
            { 
            string idHolder = Request.QueryString["i"];
            ResponseDto res = new FileUtil().ReadFromFile("module");
            if (res.responseCode.Equals("00"))
            {
                List<ModuleDto> dataList = JsonConvert.DeserializeObject<List<ModuleDto>>(res.responseMessage);
                dataObj = dataList.Find(p => p.moduleId == idHolder);
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
        public ActionResult ViewModules()
        {
            List<ModuleDto> dataList = new List<ModuleDto>();
            try
            { 
            ResponseDto res = new FileUtil().ReadFromFile("module");
            if (res.responseCode.Equals("00"))
            {
                dataList = JsonConvert.DeserializeObject<List<ModuleDto>>(res.responseMessage);
            }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return View(dataList);
        }
        public ActionResult AboutModule()
        {
            ModuleDto dataObj = new ModuleDto();
            try
            { 
            string idHolder = Request.QueryString["i"];
            ResponseDto res = new FileUtil().ReadFromFile("module");
            if (res.responseCode.Equals("00"))
            {
                List<ModuleDto> dataList = JsonConvert.DeserializeObject<List<ModuleDto>>(res.responseMessage);
                dataObj = dataList.Find(p => p.moduleId == idHolder);
            }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return View(dataObj);
        }

        [HttpPost]
        public ActionResult SubmitData(ModuleDto model)
        {
            try
            { 
            //if (ModelState.IsValid)
            {
                model.moduleId = new ProcessUtil().generateId("MODULE");
                model.dateCreated = new ProcessUtil().currentPeriod();

                ResponseDto resp = new ResponseDto();
                resp.responseCode = "00";
                resp.responseMessage = "Success";
                List<ModuleDto> dataList = new List<ModuleDto>();

                ResponseDto res = new FileUtil().ReadFromFile("module");
                if (res.responseCode.Equals("00"))
                {
                    dataList = JsonConvert.DeserializeObject<List<ModuleDto>>(res.responseMessage);

                    if (dataList.Count > 0)
                    {
                        if (dataList.Find(p => (p.moduleName == model.moduleName)) != null)
                        {
                            resp.responseCode = "01";
                            resp.responseMessage = "Data already exists";

                                new LogUtil().WriteLog("ERROR: " + resp.responseCode);
                                new LogUtil().WriteLog("ERROR: " + resp.responseMessage + " " + model.moduleName);
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

                    new FileUtil().WriteToFile(new ProcessUtil().convertToJson(dataList), "module", saveMode);
                    new ProcessUtil().updateSequence("MODULE", model.moduleId);
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