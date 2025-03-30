using Newtonsoft.Json;
using Stucation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Stucation.Controllers.Utils
{
    public class ListHelper
    {
        public SelectList ToSelectCourseList(List<CourseDto> dtList)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            try
            {
                foreach (CourseDto dat in dtList)
                {
                    list.Add(new SelectListItem()
                    {
                        Text = dat.courseName.ToString(),
                        Value = dat.courseId.ToString()
                    });
                }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return new SelectList(list, "Value", "Text");
        }
   
        public SelectList ToSelectSingleCourseList(CourseDto dtList)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            try
            {
                    list.Add(new SelectListItem()
                    {
                        Text = dtList.courseName.ToString(),
                        Value = dtList.courseId.ToString()
                    });

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return new SelectList(list, "Value", "Text");
        }
   public SelectList ToSelectSingleStudentList(StudentDto dtList)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            try
            {
                    list.Add(new SelectListItem()
                    {
                        Text = dtList.firstName.ToString(),
                        Value = dtList.studentId.ToString()
                    });

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return new SelectList(list, "Value", "Text");
        }
   

    public SelectList ToSelectModuleList(List<ModuleDto> dtList)
    {
        List<SelectListItem> list = new List<SelectListItem>();

        try
        {
            foreach (ModuleDto dat in dtList)
            {
                list.Add(new SelectListItem()
                {
                    Text = dat.moduleName.ToString(),
                    Value = dat.moduleId.ToString()
                });
            }

        }
        catch (Exception ex)
        {
            new LogUtil().WriteLog("ERROR: " + ex.Message);
            
        }
        return new SelectList(list, "Value", "Text");
    }

        public SelectList ToSelectAssessmentList(List<AssessmentDto> dtList)
    {
        List<SelectListItem> list = new List<SelectListItem>();

        try
        {
            foreach (AssessmentDto dat in dtList)
            {
                list.Add(new SelectListItem()
                {
                    Text = dat.assessmentTitle.ToString(),
                    Value = dat.assessmentId.ToString()
                });
            }

        }
        catch (Exception ex)
        {
            new LogUtil().WriteLog("ERROR: " + ex.Message);
            
        }
        return new SelectList(list, "Value", "Text");
    }

    }
}