using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using gtv_tele.Models;
using gtv_tele.CustomAuthentication;
using gtv_tele.DataAccess;

namespace gtv_tele.Controllers
{    
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {            
            return RedirectPermanent("/Account/Login");
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl="")
        {
            if (User.Identity.IsAuthenticated)
            {                
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginView loginView, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(loginView.UserName, loginView.Password))
                {
                    var user = (CustomMembershipUser)Membership.GetUser(loginView.UserName, false);                    
                    if (user != null)
                    {
                        CustomSerializeModel userModel = new Models.CustomSerializeModel()
                        {
                            Email = user.Email,
                            UserId = user.UserId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            RoleName = user.Roles.Select(r => r.RoleName).ToList(),
                            RoleId = user.Roles.Select(r => r.RoleId).ToList(),
                        };

                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                            (
                            1, loginView.UserName, DateTime.Now, DateTime.Now.AddMinutes(15), false, userData
                            );
                        
                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie faCookie = new HttpCookie("Cookie1", enTicket);
                        Response.Cookies.Add(faCookie);   
                        
                    }

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index","Dashboard");
                    }
                }
            }
            ModelState.AddModelError("", "Username or Password invalid ^_^ ");
            return View(loginView);
        }
                
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(RegistrationView registrationView)
        {
            bool statusRegistration = false;
            string messageRegistration = string.Empty;

            if (ModelState.IsValid)
            {
                // Email Verification  
                var userName = Membership.GetUserNameByEmail(registrationView.Email);
                if (!string.IsNullOrEmpty(userName))
                {
                    ModelState.AddModelError("Warning Email", "Sorry: Email already Exists");
                    messageRegistration = "Sorry: Email already Exists";
                    ViewBag.Message = messageRegistration;
                    ViewBag.Status = statusRegistration;
                    return View(registrationView);
                }
                MembershipUser checkUsername = Membership.GetUser(registrationView.Username, false);
                
                if(checkUsername != null)
                {
                    ModelState.AddModelError("Warning Username", "Sorry: Username already Exists");
                    messageRegistration = "Sorry: Username "+ checkUsername.UserName + " already Exists";
                    ViewBag.Message = messageRegistration;
                    ViewBag.Status = statusRegistration;
                    return View(registrationView);
                }

                //Save User Data   
                using (AuthenticationDB dbContext = new AuthenticationDB())
                {
                    var user = new User()
                    {
                        Username = registrationView.Username,
                        FirstName = registrationView.FirstName,
                        LastName = registrationView.LastName,
                        Email = registrationView.Email,
                        Password = registrationView.Password,
                        ActivationCode = Guid.NewGuid(),
                    };

                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                    //Verification Email  
                    VerificationEmail(registrationView.Email, user.ActivationCode.ToString());
                    messageRegistration = "Your account has been created successfully. ^_^";
                    statusRegistration = true;
                }

            }
            else
            {
                messageRegistration = "Something Wrong!";
            }
            ViewBag.Message = messageRegistration;
            ViewBag.Status = statusRegistration;

            return View(registrationView);
        }

        [HttpGet]
        public ActionResult ActivationAccount(string id)
        {
            bool statusAccount = false;
            using (AuthenticationDB dbContext = new DataAccess.AuthenticationDB())
            {
                var userAccount = dbContext.Users.Where(u => u.ActivationCode.ToString().Equals(id)).FirstOrDefault();

                if (userAccount != null)
                {
                    userAccount.IsActive = true;
                    dbContext.SaveChanges();
                    statusAccount = true;
                }
                else
                {
                    ViewBag.Message = "Something Wrong !!";
                }

            }
            ViewBag.Status = statusAccount;
            return View();
        }

        public ActionResult LogOut()
        {
            HttpCookie cookie = new HttpCookie("Cookie1", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account", null);
        }

        [NonAction]
        public void VerificationEmail(string email, string activationCode)
        {
            var url = string.Format("/Account/ActivationAccount/{0}", activationCode);
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);

            var fromEmail = new MailAddress("cmstlp@tracking.xptlp.co.id", "Activation Account - GTV Tele");
            var toEmail = new MailAddress(email);

            var fromEmailPassword = "TLP2021tlp";
            string subject = "Activation Account !";

            string body = "<br/> Please click on the following link in order to activate your account" + "<br/><a href='" + link + "'> Activation Account ! </a>";

            var smtp = new SmtpClient
            {
                Host = "tracking.xptlp.co.id",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            })

            smtp.Send(message);

        }
    }
}