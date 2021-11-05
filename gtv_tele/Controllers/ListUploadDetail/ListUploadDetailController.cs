using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gtv_tele.DataAccess;
using ClosedXML;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace gtv_tele.Controllers.ListUploadDetail
{
    [Authorize]
    public class ListUploadDetailController : Controller
    {
        AuthenticationDB dbCOntext = new AuthenticationDB();
        // GET: ListUploadDetail
        [HttpGet]

        public ActionResult Index(Master_Upload Lmu)
        {
            string tgl1 = null;
            string tgl2 = null;
            tgl1 = Request["tgl1"];
            tgl2 = Request["tgl2"];

            var datalist = (from mud in dbCOntext.Master_Upload_Detail
                            join mu in dbCOntext.Master_Upload on mud.Master_UploadId equals mu.Master_UploadId
                            join cust in dbCOntext.Customer on mu.CustomerId equals cust.CustomerId
                            join proj in dbCOntext.Project on mu.ProjectId equals proj.ProjectId
                            select new
                            {
                                mud.Master_Upload_DetailId,
                                mud.Master_UploadId,
                                mud.NoHP,
                                mud.Nama,
                                mud.Account,
                                mud.Body_Message,
                                mud.Short_Link,
                                mud.Path_PDF,
                                mud.Send,
                                mud.Send_at,
                                mud.Status,
                                mud.Message_Status,
                                mud.Read_Total,
                                mud.Read_at,
                                mud.Keterangan,
                                mu.Created_at,
                                mu.Cycle,
                                mu.Part,
                                mu.CustomerId,
                                cust.CustomerName,
                                proj.ProjectName
                            });

            if(tgl1 != "" && tgl2 != "")
            {
                string xtgl1 = tgl1 + " 00:00:00"; string xtgl2 = tgl2 + " 23:59:59";
                DateTime tgl1x = DateTime.Parse(xtgl1); DateTime tgl2x = DateTime.Parse(xtgl2);
                datalist = datalist.Where(m => m.Created_at >= tgl1x && m.Created_at <= tgl2x);
            }

            if(Lmu.CustomerId != 0)
            {
                datalist = datalist.Where(m => m.CustomerId == Lmu.CustomerId);
            }

            if(Lmu.Cycle != null)
            {
                datalist = datalist.Where(m => m.Cycle == Lmu.Cycle);
            }

            if (Lmu.Part != 0)
            {
                datalist = datalist.Where(m => m.Part == Lmu.Part);
            }

            if(Request["submit"]  == "export")
            {
                DataTable dt = new DataTable("Grid");
                dt.Columns.AddRange(new DataColumn[10] {
                    new DataColumn("Customer"),
                    new DataColumn("Project"),
                    new DataColumn("Cycle/Part"),
                    new DataColumn("No Account"),
                    new DataColumn("Nama"),
                    new DataColumn("No HP"),
                    new DataColumn("Status"),
                    new DataColumn("Message Status"),
                    new DataColumn("Send"),
                    new DataColumn("Send At")
                    //new DataColumn("Read Total")
                    //new DataColumn("Read At"),
                });

                foreach (var row in datalist)
                {
                    dt.Rows.Add(row.CustomerName,row.ProjectName, row.Cycle+'/'+row.Part, row.Account, row.Nama, row.NoHP, row.Status
                                , row.Message_Status, row.Send, row.Send_at);
                }

                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(dt);
                MemoryStream stream = new MemoryStream();
                wb.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Data Send "+DateTime.Now.ToShortDateString()+".xlsx");
            }

            var Customer = dbCOntext.Customer;
           
            ViewBag.CustomerId = Lmu.CustomerId;
            ViewBag.Cycle = Lmu.Cycle;
            ViewBag.Customer = Customer;
            ViewBag.DataList = datalist;
            ViewBag.tgl1 = tgl1;
            ViewBag.tgl2 = tgl2;
            ViewBag.Part = Lmu.Part;
            return View();
        }
    }
}