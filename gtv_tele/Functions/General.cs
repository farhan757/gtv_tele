using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using gtv_tele.DataAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace gtv_tele.Functions
{
    public class General
    {
        AuthenticationDB dbContext = new AuthenticationDB();
        public List<Customer> getCustomer(int id = 0)
        {
            List<Customer> customer = new List<Customer>();
            if(id == 0)
            {
                customer = dbContext.Customer.ToList();
            }
            else
            {
                customer = dbContext.Customer.Where(c => c.CustomerId==id).ToList();
            }
            return customer;
        }

        public List<Project> getProject(int id = 0)
        {
            List<Project> project = new List<Project>();
            if (id == 0)
            {
                project = dbContext.Project.ToList();
            }
            else
            {
                project = dbContext.Project.Where(c => c.ProjectId == id).ToList();
            }
            return project;
        }

        public List<Project> getProjectByCustomerId(int id = 0)
        {
            List<Project> project = new List<Project>();

            project = dbContext.Project.Where(c => c.CustomerId == id).ToList();
            
            return project;
        }

        public List<Master_Upload_Detail> ReadText(string nmFile, Master_Upload Master)
        {            
            StreamReader dtRead = new StreamReader(nmFile);
            string DataLine = "";

            
            while ((DataLine = dtRead.ReadLine()) != null)
            {
                string[] datacol = DataLine.Split('|');
                Master_Upload_Detail ds = new Master_Upload_Detail();

                ds.Master_UploadId = Master.Master_UploadId;
                ds.NoHP = datacol[0];
                ds.Nama = datacol[1];
                ds.Account = datacol[2];
                ds.Path_PDF = datacol[3];
                ds.Short_Link = "";
                ds.Body_Message = "";
                ds.Send = 0;
                ds.Send_at = DateTime.Now;
                ds.Status = 0;
                ds.Message_Status = "";
                ds.Read_Total = 0;
                ds.Read_at = DateTime.Now;
                ds.Keterangan = "";                
                dbContext.Master_Upload_Detail.Add(ds);
            }
            dbContext.SaveChanges();
            dtRead.Close(); 
            var tmp = dbContext.Master_Upload_Detail.Where(mud => mud.Master_UploadId == Master.Master_UploadId).ToList();

            Body_Message BodyMsg = (from p in dbContext.Project
                                    join bm in dbContext.Body_Message on p.Body_MessageId equals bm.Body_MessageId
                                    where p.ProjectId == Master.ProjectId
                                    select bm).FirstOrDefault();
            foreach (var dr in tmp)
            {
                var updt = tmp.Where(m => m.Master_Upload_DetailId == dr.Master_Upload_DetailId).FirstOrDefault();
                dr.Body_Message = BuildBodyMessage(dr, BodyMsg);                
            }
            dbContext.SaveChanges();

            return dbContext.Master_Upload_Detail.Where(mud => mud.Master_UploadId == Master.Master_UploadId).ToList();
        }

        public string BuildBodyMessage(Master_Upload_Detail data, Body_Message BM)
        {
            //var getBodyMessage
            string jso = JsonConvert.SerializeObject(data);
            dynamic jsoAr = JObject.Parse(jso);
            string Body = BM.Body;
            List<VariableToBodyDetail> settinganField = dbContext.VariableToBodyDetail.Where(mud => mud.VariableToBodyId == BM.VariableToBodyId).ToList();
            foreach(var item in settinganField)
            {
                Body = Body.Replace(item.VariableName, Convert.ToString(jsoAr[item.VariableField]));
            }
            Body = Body.Replace("{{URL}}", LinkVerifyCode(data));
            return Body;
        }

        public string LinkVerifyCode(Master_Upload_Detail dt)
        {
            string URL = "";
            
            Verify_Code vc = new Verify_Code();
            vc.Verify_CodeId = Guid.NewGuid();
            vc.Master_Upload_DetailId = dt.Master_Upload_DetailId;
            dbContext.Verify_Code.Add(vc);
            dbContext.SaveChanges();
            var url = string.Format("/Api/Index/{0}", vc.Verify_CodeId);
            //var link = "https://xptlp.co.id" + url;
            var link = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, url);
            URL = link;
            return URL;
        }

        public static List<string> GetMembers(Type type)
        {

            return type.GetProperties()
            .Select(x => x.Name).ToList();            
        }
    }
}