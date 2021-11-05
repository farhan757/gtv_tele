using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gtv_tele.DataAccess;

namespace gtv_tele.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        AuthenticationDB dbContext = new AuthenticationDB();
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string getTotalData()
        {
            var jumlah = dbContext.Master_Upload_Detail.Count();
            return jumlah.ToString();
        }
        [HttpGet]
        public string getSended()
        {
            var jumlah = dbContext.Master_Upload_Detail.Where(m => m.Status == 1).Count();
            return jumlah.ToString();
        }
        [HttpGet]
        public string getErrors()
        {
            var jumlah = dbContext.Master_Upload_Detail.Where(m => m.Status != 1).Where(m => m.Status != 0).Count();
            return jumlah.ToString();
        }
        [HttpGet]
        public string getOnProses()
        {
            var jumlah = dbContext.Master_Upload_Detail.Where(m => m.Status == 0).Count();
            return jumlah.ToString();
        }
    }
}