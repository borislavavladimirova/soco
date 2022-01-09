using Microsoft.AspNet.Identity;
using SoCoWebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoCoWebApp.Controllers
{
    public class ApplicationController : Controller
    {
        // GET: Application
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    var applications =  ctx.Application.OrderBy(c=>c.IsActive).ToList();
                    foreach (var application in applications)
                    {
                        application.ApplicantState.State = ctx.ApplicantState.Where(s => s.Id == application.ApplicantStateId).Select(s => s.State).FirstOrDefault();
                    }
                    return View(applications);                  
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // GET: Application/Details/5
        public ActionResult Details(int id)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    var application = ctx.Application.Where(c => c.Id == id).FirstOrDefault();
                    var job = ctx.Job.Where(s => s.Id == application.JobId).FirstOrDefault();

                    application.ApplicantState.State = ctx.ApplicantState.Where(s => s.Id == application.ApplicantStateId).Select(s => s.State).FirstOrDefault();
                    application.User = ctx.User.Where(s => s.Id == application.ObserverId).FirstOrDefault();
                    application.User1 = ctx.User.Where(s => s.Id == application.UpdatedBy).FirstOrDefault();                    
                    job.Office.Name = ctx.Office.Where(s => s.Id == job.OfficeId).Select(s => s.Name).FirstOrDefault();
                    job.Position.Name = ctx.Position.Where(s => s.Id == job.PositionId).Select(s => s.Name).FirstOrDefault();
                    application.Job = job;

                    return View(application);
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // GET: Application/Create
        public ActionResult Create(int jobId)
        {
            ViewBag.jobId = jobId;
            return View();
        }

        // POST: Application/Create
        [HttpPost]
        public ActionResult Create(int jobId, Application application, HttpPostedFileBase postedfile)
        {
            ViewBag.jobId = jobId;
            using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        bool isExist = ctx.Application.Any(u => u.Email == application.Email && u.JobId == jobId && u.IsActive == true);
                        if (isExist)
                        {
                            ViewBag.error = "You've already applied for this job and your application is still active.";
                            return View(application);
                        }
                        else
                        {
                            if (postedfile != null)
                            {
                                var extension = System.IO.Path.GetExtension(postedfile.FileName);
                                if (extension == ".pdf")
                                {
                                    string path = Server.MapPath("~/UploadedFiles/");
                                    if (!Directory.Exists(path))
                                    {
                                        Directory.CreateDirectory(path);
                                    }

                                    postedfile.SaveAs(path + Path.GetFileName(postedfile.FileName));
                                    application.CVPath = postedfile.FileName;
                                    application.IsActive = true;
                                    application.JobId = jobId;
                                    application.ApplicationDate = DateTime.Now;
                                    application.ApplicantStateId = 1;
                                    ctx.Application.Add(application);
                                    ctx.SaveChanges();
                                    return RedirectToAction("ActiveOffices", "Office", null);
                                }
                                else
                                {
                                    ViewBag.error = "You cannot upload a file if its format is different to '.pdf'";
                                    return View(application);
                                }
                            }
                            else
                            {
                                ViewBag.error = "File is required";
                                return View(application);
                            }
                        }
                    }
                    return View(application);
                }
                catch
                {
                   return View(application);
                }
            }
        }

        // GET: Application/Edit/5
        public ActionResult Edit(int id)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    ViewBag.states = States();
                    var application = ctx.Application.Where(s => s.Id == id).FirstOrDefault();
                    application.ApplicantState.State = ctx.ApplicantState.Where(s => s.Id == application.ApplicantStateId).Select(s => s.State).FirstOrDefault();
                   
                    return View(application);
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // POST: Application/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Application application)
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.states = States();
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            var userName = User.Identity.GetUserName();
                            var userId = ctx.User.Where(u => u.Email == userName).Select(u => u.Id).FirstOrDefault();
                            application.UpdatedAt = DateTime.Now;
                            application.UpdatedBy = userId;
                            if (application.ObserverComment != null || application.ObserverComment != "") application.ObserverId = userId;
                            ctx.Entry(application).State = System.Data.Entity.EntityState.Modified;
                            ctx.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        application.ApplicantState.State = ctx.ApplicantState.Where(s => s.Id == application.ApplicantStateId).Select(s => s.State).FirstOrDefault();
                        return View(application);
                    }
                    catch
                    {
                       return View(application);
                    }
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        public List<SelectListItem> States()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
            {
                var states = ctx.ApplicantState.ToList();

                foreach (var sub in states)
                {
                    selectListItems.Add(new SelectListItem
                    {
                        Text = sub.State,
                        Value = sub.Id.ToString()

                    });
                }
                return selectListItems;
            }
        }
    }
}
