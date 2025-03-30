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
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitData(LoginDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                }
                else
                {

                    if (model.username.Equals(WebConfigurationManager.AppSettings["adminUser"].ToString()))
                    {
                            Response.Redirect("~/Home/index");
                    }
                    else
                    {

                    }
 
                }
            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
            }
            return View("login");
        }
        
        public ActionResult Logout()
        {
            Response.Redirect("~/Login/login");
            return View();
        }
    }
}