using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gtv_tele.DataAccess;

namespace gtv_tele.Controllers
{
    public class ApiController : Controller
    {
        AuthenticationDB dbContext = new AuthenticationDB();

        [HttpGet]
        public ActionResult Index(string id)
        {

            string ok = "ok";
            ok = "ok";

            try
            {
                var confir = (from vcx in dbContext.Verify_Code
                              join mud in dbContext.Master_Upload_Detail on vcx.Master_Upload_DetailId equals mud.Master_Upload_DetailId
                              where vcx.Verify_CodeId.ToString() == id
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
                                  mud.Keterangan
                              }).FirstOrDefault();
                if (confir != null)
                {
                    var readcount = dbContext.Master_Upload_Detail.Find(confir.Master_Upload_DetailId);
                    if (TryUpdateModel(readcount))
                    {
                        readcount.Read_Total = confir.Read_Total + 1;
                        readcount.Read_at = DateTime.Now;
                    }
                    //dbContext.SaveChanges();
                    History_Read hr = new History_Read();
                    hr.Read_at = DateTime.Now;
                    hr.Master_Upload_DetailId = confir.Master_Upload_DetailId;
                    dbContext.History_Read.Add(hr);
                    dbContext.SaveChanges();
                    //return Redirect("https://xptlp.co.id");
                    if (readcount.Path_PDF != "")
                    {
                        string fullName = Server.MapPath("~" + readcount.Path_PDF);

                        byte[] fileBytes = GetFile(fullName);
                        return File(
                            fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, readcount.Path_PDF);
                    }
                    else if (readcount.Short_Link != "")
                    {
                        return Redirect(readcount.Short_Link);
                    }
                }
            }
            catch (Exception ex)
            {
                return Redirect("https://google.com");
            }
            return Redirect("https://xptlp.co.id");
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
    }
}