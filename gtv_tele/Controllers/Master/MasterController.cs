using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gtv_tele.DataAccess;

namespace gtv_tele.Controllers.Master
{
    [Authorize]
    public class MasterController : Controller
    {
        AuthenticationDB dbContext = new AuthenticationDB();
        // GET: Master
        [HttpGet]
        public ActionResult ListCustomer()
        {
            ViewBag.DataList = dbContext.Customer;
            return View();
        }

        public ActionResult FormCustomer(int id=0)
        {
            ViewBag.Customer = dbContext.Customer.Find(id);
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public JsonResult SaveCustomer(Customer cust)
        {
            int status = 200; string message = "";
            if(cust.CustomerId == 0)
            {
                try
                {
                    Customer customer = new Customer();
                    customer.CustomerName = cust.CustomerName;
                    customer.CustomerPhone = cust.CustomerPhone;
                    customer.Address = cust.Address;
                    customer.City = cust.City;
                    customer.Region = cust.Region;
                    customer.PostalCode = cust.PostalCode;
                    customer.Country = cust.Country;
                    customer.Created_at = DateTime.Now;
                    customer.Updated_at = DateTime.Now;
                    dbContext.Customer.Add(customer);
                    dbContext.SaveChanges();
                    status = 200; message = "Success Add Customer";
                }catch (Exception ex)
                {
                    status = 400; message = ex.Message;
                }
            }
            else
            {
                try
                {
                    Customer customer = dbContext.Customer.Find(cust.CustomerId);
                    customer.CustomerName = cust.CustomerName;
                    customer.CustomerPhone = cust.CustomerPhone;
                    customer.Address = cust.Address;
                    customer.City = cust.City;
                    customer.Region = cust.Region;
                    customer.PostalCode = cust.PostalCode;
                    customer.Country = cust.Country;                    
                    customer.Updated_at = DateTime.Now;
                    dbContext.SaveChanges();
                    status = 200; message = "Success Change Customer";
                }
                catch (Exception ex)
                {
                    status = 400; message = ex.Message;
                }
            }

            return Json(new { status = status, message = message });
        }

        [HttpGet]
        public JsonResult DeleteCust(int id)
        {
            int status = 200; string message = "";
            
            var cek = dbContext.Project.Where(m => m.CustomerId == id).FirstOrDefault();
            if(cek == null)
            {
                try
                {
                    var cust = dbContext.Customer.Find(id);
                    dbContext.Customer.Remove(cust);
                    dbContext.SaveChanges();
                    status = 200; message = "Success Delete Customer";
                }
                catch(Exception ex)
                {
                    status = 400; message = ex.Message;
                }
            }
            else
            {
                status = 400; message = "Gagal Delete, karena ada beberapa project dan data yang dimiliki";
            }

            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListProject()
        {
            ViewBag.DataList = dbContext.Project;
            return View();
        }

        public ActionResult FormProject(int id = 0)
        {
            ViewBag.Project = dbContext.Project.Find(id);
            ViewBag.Customer = dbContext.Customer;
            ViewBag.BodyMessage = dbContext.Body_Message;
            ViewBag.Id = id;
            return View();
        }

        [HttpGet]
        public string BodyMessg(int id)
        {
            string Body = null;

            if(id != 0)
            {
                Body =  dbContext.Body_Message.Find(id).Body;
            }

            return Body == null ? "" : Body;
        }

        [HttpGet]
        public JsonResult DeleteProj(int id)
        {
            int status = 200; string message = "";

            try
            {
                var proj = dbContext.Project.Find(id);
                dbContext.Project.Remove(proj);
                dbContext.SaveChanges();
                status = 200; message = "Success Delete Project";
            }
            catch (Exception ex)
            {
                status = 400; message = ex.Message;
            }

            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveProject(Project proj)
        {
            int status = 200; string message = "";
            if (proj.ProjectId == 0)
            {
                try
                {
                    Project project = new Project();
                    project.ProjectName = proj.ProjectName;
                    project.ProjectDesc = proj.ProjectDesc;
                    project.CustomerId = proj.CustomerId;
                    project.Body_MessageId = proj.Body_MessageId;
                    project.Created_at = DateTime.Now;
                    project.Updated_at = DateTime.Now;
                    dbContext.Project.Add(project);
                    dbContext.SaveChanges();
                    status = 200; message = "Success Add Project";
                }
                catch (Exception ex)
                {
                    status = 400; message = ex.Message;
                }
            }
            else
            {
                try
                {
                    Project project = dbContext.Project.Find(proj.ProjectId);
                    project.ProjectName = proj.ProjectName;
                    project.ProjectDesc = proj.ProjectDesc;
                    project.CustomerId = proj.CustomerId;
                    project.Body_MessageId = proj.Body_MessageId;                    
                    project.Updated_at = DateTime.Now;
                    dbContext.SaveChanges();
                    status = 200; message = "Success Change Customer";
                }
                catch (Exception ex)
                {
                    status = 400; message = ex.Message;
                }
            }

            return Json(new { status = status, message = message });
        }

        [HttpGet]
        public ActionResult ListSender()
        {
            ViewBag.DataList = dbContext.SettingSender;
            return View();
        }

        [HttpGet]
        public JsonResult DeleteSetSend(int id)
        {
            int status = 200; string message = "";

            try
            {
                var setsend = dbContext.SettingSender.Find(id);
                dbContext.SettingSender.Remove(setsend);
                dbContext.SaveChanges();
                status = 200; message = "Success Delete Project";
            }
            catch (Exception ex)
            {
                status = 400; message = ex.Message;
            }

            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FormSetSender(int id = 0)
        {
            ViewBag.SettingSender = dbContext.SettingSender.Find(id);
            ViewBag.Id = id;
            return View();
        }

        public JsonResult SaveSetSender(DataAccess.SettingSender setsend)
        {
            int status = 200; string message = "";
            if (setsend.SettingSenderId == 0)
            {
                try
                {
                    DataAccess.SettingSender setting = new DataAccess.SettingSender();
                    setting.SettingSenderName = setsend.SettingSenderName;
                    setting.Number_Sender = setsend.Number_Sender;
                    setting.ApiId_Sender = setsend.ApiId_Sender;
                    setting.ApiHash_Sender = setsend.ApiHash_Sender;
                    dbContext.SettingSender.Add(setting);
                    dbContext.SaveChanges();
                    status = 200; message = "Success Add Sender";
                }
                catch (Exception ex)
                {
                    status = 400; message = ex.Message;
                }
            }
            else
            {
                try
                {
                    DataAccess.SettingSender setting = dbContext.SettingSender.Find(setsend.SettingSenderId);
                    setting.SettingSenderName = setsend.SettingSenderName;
                    setting.Number_Sender = setsend.Number_Sender;
                    setting.ApiId_Sender = setsend.ApiId_Sender;
                    setting.ApiHash_Sender = setsend.ApiHash_Sender;
                    dbContext.SaveChanges();
                    status = 200; message = "Success Change Sender";
                }
                catch (Exception ex)
                {
                    status = 400; message = ex.Message;
                }
            }

            return Json(new { status = status, message = message });
        }
    }
}