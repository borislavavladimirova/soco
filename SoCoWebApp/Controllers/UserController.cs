using Microsoft.AspNet.Identity;
using SoCoWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SoCoWebApp.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    var userName = User.Identity.GetUserName();
                    var user = ctx.User.Where(s => s.Email == userName).FirstOrDefault();
                    if (user.IsAdmin)
                    {
                        var users = ctx.User.ToList();
                        return View(ctx.User.ToList());
                    }
                    else return View("NotAuthorise");
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user, string returnUrl)
        {
            using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
            {
                if (ModelState.IsValid)
                {
                    var pass = Encrypt(user.Password);
                    bool IsValidUser = ctx.User.Any(u => u.Email == user.Email && u.Password == pass);

                    if (IsValidUser)
                    {
                        user = ctx.User.Where(u => u.Email == user.Email && u.Password == pass).SingleOrDefault();
                        if (user.IsActive)
                        {
                            FormsAuthentication.SetAuthCookie(user.Email, false);
                            if (returnUrl == "") return RedirectToAction("Index", "Home");
                            else return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            ViewBag.error = "This user is not active";
                            ViewBag.returnUrl = returnUrl;
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.error = "Wrong email or password";
                        ViewBag.returnUrl = returnUrl;
                        return View();
                    }
                }

                ViewBag.returnUrl = returnUrl;
                return View();
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult Manage()
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    var userName = User.Identity.GetUserName();
                    var user = ctx.User.Where(s => s.Email == userName).FirstOrDefault();                    
                    ViewBag.isAdmin = user.IsAdmin;
                    return View();
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        public ActionResult UserInfo()
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    var userName = User.Identity.GetUserName();
                    return View(ctx.User.Where(s => s.Email == userName).FirstOrDefault());
                }
            }
            else return RedirectToAction("Login", new { returnUrl = Request.Url.AbsolutePath });
        }

        public ActionResult ChangePass()
        {
            if (Request.IsAuthenticated) return View();
            else return RedirectToAction("Login", new { returnUrl = Request.Url.AbsolutePath });
        }
        
        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePass(ChangeUserPass collection)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    var userName = User.Identity.GetUserName();
                    var user = ctx.User.Where(s => s.Email == userName).FirstOrDefault();
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            if (collection.Email != userName)
                            {
                                ViewBag.error = "Invalid email adress";
                                return View();
                            }
                            else
                            {
                                if (Encrypt(collection.OldPassword) == user.Password)
                                {
                                    user.Password = Encrypt(collection.NewPassword);
                                    ctx.Entry(user).State = System.Data.Entity.EntityState.Modified;
                                    ctx.SaveChanges();
                                    return RedirectToAction("Logout");
                                }
                                else
                                {
                                    ViewBag.error = "Wrong password";
                                    return View();
                                }
                            }
                        }
                        return View(collection);
                    }
                    catch
                    {
                        return View(collection);
                    }
                }
            }
            else return RedirectToAction("Login", new { returnUrl = Request.Url.AbsolutePath });
        }

        // GET: User/Create
        public ActionResult Create()
        {
            if(Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    var userName = User.Identity.GetUserName();
                    var user = ctx.User.Where(s => s.Email == userName).FirstOrDefault();                    
                    if (user.IsAdmin) return View();
                    else return View("NotAuthorise");
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Registration collection)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    var userName = User.Identity.GetUserName();
                    var user = ctx.User.Where(s => s.Email == userName).FirstOrDefault();
                    if (user.IsAdmin)
                    {
                        try
                        {
                            if (ModelState.IsValid)
                            {
                                bool isEmailTaken = ctx.User.Any(u => u.Email == collection.Email);
                                if (!(isEmailTaken))
                                {
                                    User newUser = new User();
                                    newUser.Email = collection.Email;
                                    newUser.FName = collection.FName;
                                    newUser.LName = collection.LName;
                                    newUser.IsActive = collection.IsActive;
                                    newUser.IsAdmin = collection.IsAdmin;
                                    var newpass = System.Web.Security.Membership.GeneratePassword(10, 0);
                                    newUser.Password = Encrypt(newpass);
                                    ctx.User.Add(newUser);
                                    ctx.SaveChanges();
                                    GenerateMail(newUser.Email, newUser.FName + " " + newUser.LName, newpass, false);
                                    ViewBag.isForgotten = false;
                                    return View("PassReset");
                                }
                                else
                                {
                                    ViewBag.error = "This email is taken";
                                    return View(collection);
                                }                               
                            }
                            return View(collection);
                        }
                        catch
                        {
                            return View(collection);
                        }
                    }
                    else return View("NotAuthorise");
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        //EditUserInfo
        public ActionResult EditUserInfo(int id)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                  return View(ctx.User.Where(s => s.Id == id).FirstOrDefault());
                }
            }
            else return RedirectToAction("Login", new { returnUrl = Request.Url.AbsolutePath });
        }


        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserInfo(int id, User collection)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            ctx.Entry(collection).State = System.Data.Entity.EntityState.Modified;
                            ctx.SaveChanges();
                            return RedirectToAction("UserInfo");
                        }
                        return View(collection);
                    }
                    catch
                    {
                        return View(collection);
                    }
                }
            }
            else return RedirectToAction("Login", new { returnUrl = Request.Url.AbsolutePath });
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    var currUserName = User.Identity.GetUserName();
                    var userToEdit = ctx.User.Where(s => s.Id == id).FirstOrDefault();
                    var curruser = ctx.User.Where(s => s.Email == currUserName).FirstOrDefault();

                    if (curruser.IsAdmin) return View(userToEdit);                    
                    else return View("NotAuthorise");
                }
            }
            else return RedirectToAction("Login", new { returnUrl = Request.Url.AbsolutePath });
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User collection)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    var currUserName = User.Identity.GetUserName();
                    var curruser = ctx.User.Where(s => s.Email == currUserName).FirstOrDefault();
                    if (curruser.IsAdmin)
                    {
                        try
                        {
                            if (ModelState.IsValid)
                            {
                                ctx.Entry(collection).State = System.Data.Entity.EntityState.Modified;
                                ctx.SaveChanges();
                                return RedirectToAction("Index");
                            }
                            return View(collection);
                        }
                        catch
                        {
                            return View(collection);
                        }
                    }
                    else return View("NotAuthorise");
                }
            }
            else return RedirectToAction("Login", new { returnUrl = Request.Url.AbsolutePath });
        }

        public ActionResult ForgottenPassword()
        {
            return View();
        }

        // POST: User/forgottenpass/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgottenPassword(ForgottenPass collection)
        {
            using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var user = ctx.User.Where(s => s.Email == collection.Email).FirstOrDefault();

                        if (user != null)
                        {
                            var newpass = System.Web.Security.Membership.GeneratePassword(10, 0);                          
                            user.Password = Encrypt(newpass);
                            ctx.Entry(user).State = System.Data.Entity.EntityState.Modified;
                            ctx.SaveChanges();
                            GenerateMail(user.Email, user.FName + " " + user.LName, newpass, true);
                            ViewBag.isForgotten = true;
                            ViewBag.user = user.Email;
                            return View("PassReset");
                        }
                        ViewBag.error = "User with this email does not exist.";
                        return View(collection);
                    }
                    return View(collection);
                }
                catch
                {
                    return View(collection);
                }
            }
        }

        public string Encrypt(string value)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                UTF8Encoding utf8 = new UTF8Encoding();
                byte[] data = md5.ComputeHash(utf8.GetBytes(value));
                return Convert.ToBase64String(data);
            }
        }

        public ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public static void GenerateMail(string email, string name, string pass, bool isForgottenPass)
        {
            try
            {
                string subject = isForgottenPass ? "Reset password" : "New registration";
                string text = isForgottenPass ? "Your password's been reset." : "You've been granted access to the SoCo site.";
                string body = "Hello, " + name + "  <br /><br />" + text +
                " <br />Your password for SoCo site is: " + pass + " <br />Please change the password as soon as posible!<br /><br />" +
                "Kind regards, <br />SoCo support team";
                try
                {
                    SendEmail(body, subject, email);
                }
                catch (Exception e)
                {

                }
            }
            catch (Exception ex)
            {

            }

        }

        public static void SendEmail(string body, string subject, string email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("enividnyarska@gmail.com");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = body;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("enividnyarska", "eniVidnNyarska_8!");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

            }
            catch (Exception ex)
            {

            }
        }
    }
}
