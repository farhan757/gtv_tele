using gtv_tele.Functions;
using gtv_tele.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using TeleSharp.TL.Contacts;
using TeleSharp.TL;
using TLSharp.Core;
using static System.Net.WebRequestMethods;
using gtv_tele.Models;
using gtv_tele.DataAccess;
using Microsoft.Ajax.Utilities;
using gtv_tele.TagHelpers;
using System.Data.Entity;
using gtv_tele.CustomAuthentication;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Security.Policy;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Drawing.Charts;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace gtv_tele.Controllers.SenderTL
{

    [Authorize]
    public class SenderController : Controller
    {
        General fg = new General();
        AuthenticationDB dbContext = new AuthenticationDB();
        private string appDataFolder;
        private FileSessionStore store;
        private TelegramClient client;
        private string phoneNumber = "6281574965152";//string.Empty;
        private string userAttemptsFileFullPath = string.Empty;
        private string hash = string.Empty;
        private int userAttempts;
        private List<string> randomUsernames = new List<string>();
        CustomPrincipal user = (CustomPrincipal)System.Web.HttpContext.Current.User;
        // GET: Sender
        [HttpGet]
        public ActionResult FormSender()
        {
            ViewBag.Customer = fg.getCustomer();
            ViewBag.NumberSender = dbContext.SettingSender.ToList();
            return View();
        }

        public ActionResult getProjectByCust(int id = 0)
        {
            List<Project> project = fg.getProjectByCustomerId(id);

            return Json(project, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> ConnectAregis(string number)
        {
            TelegramClient client = this.ConfirmTelegram(number);
            string message = ""; int status = 200; int codehash = 400;
            try
            {
                client.ConnectAsync().Wait(900000);
                message = "Koneksi Berhasil ..";
                status = 200;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                status = 400;
            }

            if (this.client.IsUserAuthorized())
            {
                message = "Successfully Register and Connect";
                status = 200;
            }
            else
            {
                try
                {
                    string str = await client.SendCodeRequestAsync(number);
                    this.hash = str;
                    message = this.hash;
                    status = 200;
                    codehash = 200;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    status = 400;
                }
            }
            this.CreateUserAttemptsSession();
            return Json(new { authorize = codehash, status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetOTP(string number)
        {
            string message = ""; int status = 200;
            TelegramClient client = this.ConfirmTelegram(number);

            try
            {
                client.ConnectAsync().Wait(1000);
                status = 200;
                message = "Koneksi Berhasil ..";
            }
            catch (Exception ex)
            {
                status = 400;
                message = ex.Message;
            }

            try
            {
                string str = await client.SendCodeRequestAsync(number);
                this.hash = str;
                message = this.hash;
                status = 200;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                status = 400;
            }
            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> SendCode(string number, string hash, string code)
        {
            string message = ""; int status = 200;
            TelegramClient client = this.ConfirmTelegram(number);

            try
            {
                client.ConnectAsync().Wait(1000);
                status = 200;
                message = "Koneksi Berhasil ..";
            }
            catch (Exception ex)
            {
                status = 400;
                message = ex.Message;
            }

            TelegramClient clients = this.client;
            try
            {
                await clients.MakeAuthAsync(number, hash, code);
                status = 200;
                message = "Berhasil Authentication ..";
            }
            catch (Exception ex)
            {
                status = 400;
                message = ex.Message;
            }
            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> FormSender(SenderView sv)
        {
            ViewBag.Customer = fg.getCustomer();
            ViewBag.NumberSender = dbContext.SettingSender.ToList();

            TelegramClient client = this.ConfirmTelegram(sv.number);
            string message = ""; int status = 200; int codehash = 400;

            try
            {                
                client.ConnectAsync().Wait(1000);
                message = "Koneksi Berhasil ..";
                status = 200;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                status = 400;
            }

            if (this.client.IsUserAuthorized())
            {
                message = "Successfully Register and Connect";
                status = 200;
                int MasterId = 0;
                try
                {
                    //Use Namespace called :  System.IO  
                    string FileName = Path.GetFileName(sv.TxtFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), FileName);
                    sv.TxtFile.SaveAs(path);

                    Master_Upload mu = new Master_Upload();
                    mu.CustomerId = sv.Customer;
                    mu.ProjectId = sv.Project;
                    mu.Cycle = sv.Cycle;
                    mu.Part = sv.Part;
                    mu.FileName = FileName;
                    mu.FilePath = path;
                    mu.UserId = user.UserId;
                    mu.Created_at = DateTime.Now;
                    mu.Updated_at = DateTime.Now;
                    dbContext.Master_Upload.Add(mu);
                    dbContext.SaveChanges();
                    MasterId = mu.Master_UploadId;


                    int cntr = 0;
                    List<Master_Upload_Detail> DataList = fg.ReadText(path, mu);
                    
                    foreach (Master_Upload_Detail dt in DataList)
                    {                                               
                        cntr++;
                        if (cntr == 5)
                        {
                            string ok = "s";
                            ok = "6";
                        }
                        var result = await client.GetContactsAsync();
                        var users = result.Users.Where(x => x.GetType() == typeof(TLUser))
                                    .Cast<TLUser>()
                                    .FirstOrDefault(x => x.Phone == dt.NoHP);
                        if (await client.IsPhoneRegisteredAsync(dt.NoHP))
                        {
                            if (users == null)
                            {
                                var contacts = new TLVector<TLInputPhoneContact>();
                                contacts.Add(new TLInputPhoneContact
                                {
                                    FirstName = dt.Nama,
                                    LastName = dt.Account,
                                    Phone = dt.NoHP
                                }
                                );
                                var req = new TLRequestImportContacts() { Contacts = contacts };
                                var test = await client.SendRequestAsync<TLImportedContacts>(req);

                                long mid = req.MessageId;
                                result = await client.GetContactsAsync();
                                users = result.Users.Where(x => x.GetType() == typeof(TLUser))
                                        .Cast<TLUser>()
                                        .FirstOrDefault(x => x.Phone == dt.NoHP);
                            }

                            if (users != null)
                            {
                                try
                                {
                                    var ok = await client.SendMessageAsync(new TLInputPeerUser() { UserId = users.Id }, dt.Body_Message);

                                    var mud = dbContext.Master_Upload_Detail.Find(dt.Master_Upload_DetailId);
                                    if (TryUpdateModel(mud))
                                    {
                                        mud.Send = 1;
                                        mud.Send_at = DateTime.Now;
                                        mud.Status = 1;
                                        mud.Message_Status = "Successfully";
                                        //dbContext.SaveChanges();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    var mud = dbContext.Master_Upload_Detail.Find(dt.Master_Upload_DetailId);
                                    if (TryUpdateModel(mud))
                                    {
                                        mud.Send = 2;
                                        mud.Send_at = DateTime.Now;
                                        mud.Status = 2;
                                        mud.Message_Status = ex.Message;
                                        //dbContext.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                var mud = dbContext.Master_Upload_Detail.Find(dt.Master_Upload_DetailId);
                                if (TryUpdateModel(mud))
                                {
                                    mud.Send = 3;
                                    mud.Send_at = DateTime.Now;
                                    mud.Status = 3;
                                    mud.Message_Status = "Number not register Telegram";
                                    //dbContext.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            var mud = dbContext.Master_Upload_Detail.Find(dt.Master_Upload_DetailId);
                            if (TryUpdateModel(mud))
                            {
                                mud.Send = 3;
                                mud.Send_at = DateTime.Now;
                                mud.Status = 3;
                                mud.Message_Status = "Number not register Telegram";
                                //dbContext.SaveChanges();
                            }
                        }

                        Thread.Sleep(10000);                       
                    }
                   
                    message = "Done Sending Data using Telegram GTV";
                    status = 200;

                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    status = 400;

                    var mu = dbContext.Master_Upload.Where(m => m.Master_UploadId == MasterId).FirstOrDefault();
                    dbContext.Master_Upload.Remove(mu);

                    var mud = dbContext.Master_Upload_Detail.Where(m => m.Master_UploadId == MasterId).ToList();
                    foreach (var val in mud)
                    {
                        dbContext.Master_Upload_Detail.Remove(val);
                    }
                    //dbContext.SaveChanges();
                }
            }
            else
            {
                message = "not Authorize";
                status = 400;
            }
            dbContext.SaveChanges();

            this.SaveToUserAttemptsFile();
            return Json(new { status = status, message = message });
        }

        public void CreateDirectoryForSessions()
        {
            appDataFolder = Server.MapPath("~/App_Data/TelegramsenderData/Sessions");
            string path = Path.Combine(this.appDataFolder);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            DirectoryInfo basePath = new DirectoryInfo(path);
            DirectorySecurity accessControl = basePath.GetAccessControl();
            accessControl.AddAccessRule(new FileSystemAccessRule((IdentityReference)new SecurityIdentifier(WellKnownSidType.WorldSid, (SecurityIdentifier)null), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            basePath.SetAccessControl(accessControl);
            this.store = new FileSessionStore(basePath);
        }

        public void CreateUserAttemptsSession()
        {
            appDataFolder = Server.MapPath("~/App_Data");
            this.userAttemptsFileFullPath = Path.Combine(Path.Combine(this.appDataFolder), this.phoneNumber + "AT.dat");
            if (!System.IO.File.Exists(this.userAttemptsFileFullPath))
            {
                System.IO.File.Create(this.userAttemptsFileFullPath).Close();
                this.GrantAccess(this.userAttemptsFileFullPath);
                System.IO.File.WriteAllBytes(this.userAttemptsFileFullPath, BitConverter.GetBytes(5));
                this.userAttempts = 5;
            }
            else
                this.userAttempts = BitConverter.ToInt32(System.IO.File.ReadAllBytes(this.userAttemptsFileFullPath), 0);
        }

        public void GrantAccess(string fullPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
            DirectorySecurity accessControl = directoryInfo.GetAccessControl();
            accessControl.AddAccessRule(new FileSystemAccessRule((IdentityReference)new SecurityIdentifier(WellKnownSidType.WorldSid, (SecurityIdentifier)null), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            directoryInfo.SetAccessControl(accessControl);
        }

        private void SaveToUserAttemptsFile()
        {
            if (!System.IO.File.Exists(this.userAttemptsFileFullPath))
                return;
            System.IO.File.WriteAllBytes(this.userAttemptsFileFullPath, BitConverter.GetBytes(this.userAttempts));
        }

        private TelegramClient ConfirmTelegram(string number)
        {
            DataAccess.SettingSender settingan = dbContext.SettingSender.Where(s => s.Number_Sender == number).FirstOrDefault();
            this.client = new TelegramClient(settingan.ApiId_Sender, settingan.ApiHash_Sender, (ISessionStore)this.store, settingan.Number_Sender.ToString(), null, DataCenterIPVersion.PreferIPv4);
            return this.client;
        }


    }
}