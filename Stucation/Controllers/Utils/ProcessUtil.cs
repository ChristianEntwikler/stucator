using Newtonsoft.Json;
using Stucation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stucation.Controllers.Utils
{
    public class ProcessUtil
    {
        public string generateId(string idType)
        {
            string resp = "";
            try
            {
                DateTime datevar = DateTime.Now;

                ResponseDto res = new FileUtil().ReadFromFile("sequence");
                SequenceListDto seqdata = JsonConvert.DeserializeObject<SequenceListDto>(res.responseMessage);

                if (idType.Equals("STUDENT"))
                {
                    SequenceDto seq = seqdata.data.FirstOrDefault(p => p.sequenceType == "STUDENT");
                    String dt = datevar.ToString("yyyyMMdd");
                    String idSuffix = sequenceIncrease(seq);
                    resp = dt + idSuffix;
                }
                else if (idType.Equals("COURSE") || idType.Equals("MODULE") || idType.Equals("ASSESSMENT"))
                {
                    SequenceDto seq = seqdata.data.FirstOrDefault(p => p.sequenceType == idType);
                    resp = sequenceIncrease(seq);
                }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return resp;
        }

        public string sequenceIncrease(SequenceDto seq)
        {
            string resp = seq.recentSequence;
            try
            {
                int seqSuffix = int.Parse(seq.recentSequence);
                if ((seqSuffix + 1).ToString().Length <= seq.initialSequence.Length)
                {
                    seqSuffix += 1;
                    string suffValue = seqSuffix.ToString();

                    if (seq.initialSequence.Length > suffValue.Length)
                    {
                        string suffExtra = "";
                        while ((seq.initialSequence.Length - (suffValue.Length + suffExtra.Length)) > 0)
                        {
                            suffExtra = suffExtra + "0";
                        }

                        suffValue = suffExtra + suffValue;
                    }

                    resp = suffValue;
                }

            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
            return resp;
        }

        public string currentPeriod()
        {
            DateTime datevar = DateTime.Now;

            return datevar.ToString("yyyy-MM-dd hh:mm");
        }

        public string convertToJson(object req)
        {
            string resp = "";
            try
            {
                resp = JsonConvert.SerializeObject(req);
            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }

            return resp;
        }

        public void updateSequence(string sequenceType, string dataId)
        {
            try
            {
                ResponseDto resp = new FileUtil().ReadFromFile("sequence");
                SequenceListDto seqdata = JsonConvert.DeserializeObject<SequenceListDto>(resp.responseMessage);
                SequenceDto seq = seqdata.data.FirstOrDefault(p => p.sequenceType == sequenceType);
                seq.recentSequence = dataId;
                int index = seqdata.data.FindIndex(p => p.sequenceType == sequenceType);
                if (index >= 0)
                {
                    seqdata.data[index] = seq;
                }
                new FileUtil().WriteToFile(new ProcessUtil().convertToJson(seqdata), "sequence", "UPDATE");
            }
            catch (Exception ex)
            {
                new LogUtil().WriteLog("ERROR: " + ex.Message);
                
            }
        }

        public StudentDto fetchStudentDetails(string studentId)
        {
            StudentDto resp = new StudentDto();
            ResponseDto res = new FileUtil().ReadFromFile("student");
            if (res.responseCode.Equals("00"))
            {
                List<StudentDto> dataList = JsonConvert.DeserializeObject<List<StudentDto>>(res.responseMessage);
                resp = dataList.Find(p => p.studentId == studentId);
            }

            return resp;
        }

        public ModuleDto fetchModuleDetails(string moduleId)
        {
            ModuleDto resp = new ModuleDto();
            ResponseDto res = new FileUtil().ReadFromFile("module");
            if (res.responseCode.Equals("00"))
            {
                List<ModuleDto> dataList = JsonConvert.DeserializeObject<List<ModuleDto>>(res.responseMessage);
                resp = dataList.Find(p => p.moduleId == moduleId);
            }

            return resp;
        }


        public AssessmentDto fetchAssessmentDetails(string assessmentId)
        {
            AssessmentDto resp = new AssessmentDto();
            ResponseDto res = new FileUtil().ReadFromFile("assessment");
            if (res.responseCode.Equals("00"))
            {
                List<AssessmentDto> dataList = JsonConvert.DeserializeObject<List<AssessmentDto>>(res.responseMessage);
                resp = dataList.Find(p => p.assessmentId == assessmentId);
            }

            return resp;
        }
        public CourseDto fetchCourseDetails(string courseId)
        {
            CourseDto resp = new CourseDto();
            ResponseDto res = new FileUtil().ReadFromFile("course");
            if (res.responseCode.Equals("00"))
            {
                List<CourseDto> dataList = JsonConvert.DeserializeObject<List<CourseDto>>(res.responseMessage);
                resp = dataList.Find(p => p.courseId == courseId);
            }

            return resp;
        }



    }

}