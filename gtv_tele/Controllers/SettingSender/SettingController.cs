using gtv_tele.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using gtv_tele.DataAccess;
using gtv_tele.Functions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace gtv_tele.Controllers.SettingSender
{
    [Authorize]
    public class SettingController : Controller
    {
        AuthenticationDB dbContext = new AuthenticationDB();
        // GET: Setting
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.DataList = dbContext.VariableToBody;
            return View();
        }

        [HttpGet]
        public ActionResult FormVarToField(int id=0)
        {
            ViewBag.VariableToBodyName = dbContext.VariableToBody.Where(m => m.VariableToBodyId == id).FirstOrDefault();
            ViewBag.DataList = dbContext.VariableToBodyDetail.Where(m => m.VariableToBodyId == id);


            ViewBag.FieldName = General.GetMembers(typeof(Master_Upload_Detail));
            ViewBag.Id = id;    
            return View();
        }

        [HttpPost]
        public JsonResult SaveData(List<string> VariableName =null, List<string> VariableField=null)
        {
            long id = Int64.Parse(Request["id"]);
            string nama = Request["VariableToBodyName"];
            string message = ""; int status = 200;
            if(id == 0)
            {                
                try
                {
                    VariableToBody vtb = new VariableToBody();
                    vtb.VariableToBodyName = nama;
                    dbContext.VariableToBody.Add(vtb);
                    dbContext.SaveChanges();
                    id = vtb.VariableToBodyId;
                    for (int i = 0; i < VariableName.Count; i++)
                    {
                        VariableToBodyDetail vtbd = new VariableToBodyDetail();
                        string vaf = VariableField[i];
                        string van = VariableName[i];
                        vtbd.VariableToBodyId = vtb.VariableToBodyId;
                        vtbd.VariableName = van;
                        vtbd.VariableField = vaf;
                        dbContext.VariableToBodyDetail.Add(vtbd);
                    }
                    dbContext.SaveChanges();

                    status = 200; message = "Success Add Data";
                }
                catch(Exception ex)
{
                    message = ex.Message;
                    status = 400;

                    var mu = dbContext.VariableToBody.Where(m => m.VariableToBodyId == id).FirstOrDefault();
                    dbContext.VariableToBody.Remove(mu);

                    var mud = dbContext.VariableToBodyDetail.Where(m => m.VariableToBodyId == id).ToList();
                    foreach (var val in mud)
                    {
                        dbContext.VariableToBodyDetail.Remove(val);
                    }
                    dbContext.SaveChanges();
                }
            }
            else
            {
                try
                {
                    var vtb = dbContext.VariableToBody.Find(id);
                    vtb.VariableToBodyName = nama;                    
                    dbContext.SaveChanges();

                    var vtbdt = dbContext.VariableToBodyDetail.Where(m => m.VariableToBodyId == id).ToList();
                    foreach (var val in vtbdt)
                    {
                        dbContext.VariableToBodyDetail.Remove(val);
                    }
                    dbContext.SaveChanges();

                    for (int i = 0; i < VariableName.Count; i++)
                    {
                        VariableToBodyDetail vtbd = new VariableToBodyDetail();
                        string vaf = VariableField[i];
                        string van = VariableName[i];
                        vtbd.VariableToBodyId = vtb.VariableToBodyId;
                        vtbd.VariableName = van;
                        vtbd.VariableField = vaf;
                        dbContext.VariableToBodyDetail.Add(vtbd);
                    }
                    dbContext.SaveChanges();

                    status = 200; message = "Success Change Data";
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    status = 400;
                }
            }

            return Json(new { status = status, message = message });
        }

        public JsonResult DeleteVar(int id)
        {
            int status = 200; string message = "Success";
            try
            {
                var VB = dbContext.VariableToBody.Where(m => m.VariableToBodyId == id).FirstOrDefault();
                dbContext.VariableToBody.Remove(VB);

                var VBD = dbContext.VariableToBodyDetail.Where(m => m.VariableToBodyId == id).ToList();
                foreach (var val in VBD)
                {
                    dbContext.VariableToBodyDetail.Remove(val);
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                status = 400;
                message = e.Message;
            }
            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ListBodyMsg()
        {
            ViewBag.DataList = dbContext.Body_Message;

            return View();
        }

        public ActionResult FormBodyMsg(int id =0)
        {
            ViewBag.Body_Message = dbContext.Body_Message.Find(id);
            ViewBag.VariableBody = dbContext.VariableToBody;
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public JsonResult SaveBodyMsg()
        {
            string message = ""; int status = 200;
            long Body_MessageId = Int64.Parse(Request["Body_MessageId"]);
            string Nama_Body = Request["Nama_Body"];
            int VariableToBodyId = int.Parse(Request["VariableToBodyId"]);
            String Body = Request["Body"];
            if (Body_MessageId == 0)
            {
                try
                {
                    Body_Message bm = new Body_Message();
                    bm.Nama_Body = Nama_Body;
                    bm.VariableToBodyId = VariableToBodyId;
                    bm.Body = Body;
                    dbContext.Body_Message.Add(bm);
                    dbContext.SaveChanges();

                    status = 200; message = "Success Add Data";
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    status = 400;
                }
            }
            else
            {
                try
                {
                    var bmu = dbContext.Body_Message.Find(Body_MessageId);
                    bmu.Body = Body;
                    bmu.Nama_Body = Nama_Body;
                    bmu.VariableToBodyId = VariableToBodyId;
                    dbContext.SaveChanges();

                    status = 200; message = "Success Change Data";
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    status = 400;
                }
            }

            return Json(new { status = status, message = message });
        }

        [HttpGet]
        public JsonResult DeleteBody(int id)
        {
            int status = 200; string message = "Success";
            var cek = dbContext.Project.Where(m => m.Body_MessageId == id).FirstOrDefault();
            if(cek == null)
            {
                try
                {
                    var VB = dbContext.Body_Message.Where(m => m.Body_MessageId == id).FirstOrDefault();
                    dbContext.Body_Message.Remove(VB);

                    dbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    status = 400;
                    message = e.Message;
                }
            }
            else
            {
                status = 400;
                message = "Tidak bisa delete, karena body message masih digunakan dibeberapa project";
            }

            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getVardetail(int id)
        {
            var data = dbContext.VariableToBodyDetail.Where(m => m.VariableToBodyId == id).ToList();
            return Json(new { data = data}, JsonRequestBehavior.AllowGet);
        }
    }
}