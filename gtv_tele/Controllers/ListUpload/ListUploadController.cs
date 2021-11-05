using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using gtv_tele.DataAccess;
namespace gtv_tele.Controllers.ListUpload
{
    [Authorize]
    public class ListUploadController : Controller
    {
        AuthenticationDB dbContext = new AuthenticationDB();
        // GET: ListUpload
        public ActionResult Index()
        {
            var datax = (from MU in dbContext.Master_Upload
                         join PJ in dbContext.Project on MU.ProjectId equals PJ.ProjectId
                         join CU in dbContext.Customer on PJ.CustomerId equals CU.CustomerId
                         join US in dbContext.Users on MU.UserId equals US.UserId
                         select new
                         {
                             CustomerName = CU.CustomerName,
                             ProjectName = PJ.ProjectName,
                             Cycle = MU.Cycle,
                             Master_UploadId = MU.Master_UploadId,
                             Part = MU.Part,
                             FileName = MU.FileName,
                             FilePath = MU.FilePath,
                             UserId = MU.UserId,
                             UserName = (US.FirstName + " " + US.LastName).Trim(),
                             Created_at = MU.Created_at,
                             Updated_at = MU.Updated_at,
                         });
            ViewBag.ListUpload = datax;
            return View();
        }

        public ActionResult Detail(int id=0)
        {
            Master_Upload master = dbContext.Master_Upload.Where(m => m.Master_UploadId == id).FirstOrDefault();
            List<Master_Upload_Detail> data = dbContext.Master_Upload_Detail.Where(m => m.Master_UploadId == id).ToList();
            ViewBag.DataDetail = data;
            ViewBag.DataMaster = master;
            return View();
        }

        public JsonResult Delete(int id =0)
        {
            int status = 200; string message = "Success";
            try
            {
                var mu = dbContext.Master_Upload.Where(m => m.Master_UploadId == id).FirstOrDefault();
                dbContext.Master_Upload.Remove(mu);                

                var mud = dbContext.Master_Upload_Detail.Where(m => m.Master_UploadId == id).ToList();
                foreach(var val in mud)
                {
                    dbContext.Master_Upload_Detail.Remove(val);                    
                    var hr = dbContext.History_Read.Where(mhr => mhr.Master_Upload_DetailId == val.Master_Upload_DetailId).ToList();
                    foreach (var vhar in hr)
                    {
                        dbContext.History_Read.Remove(vhar);                        
                    }
                    var vc = dbContext.Verify_Code.Where(mvc => mvc.Master_Upload_DetailId == val.Master_Upload_DetailId).ToList();
                    foreach (var vvc in vc)
                    {
                        dbContext.Verify_Code.Remove(vvc);                        
                    }
                }
                dbContext.SaveChanges();
            }catch(Exception e)
            {
                status = 400;
                message = e.Message;
            }
            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}