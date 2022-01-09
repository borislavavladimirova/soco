using Microsoft.AspNet.Identity;
using SoCoWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoCoWebApp.Controllers
{
    public class JobController : Controller
    {
        // GET: Job
        public ActionResult Index(int? officeid)
        {           
            using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
            {
                var jobs = (officeid == null) ? ctx.Job.ToList() : ctx.Job.Where(c => c.OfficeId == officeid).ToList();
                foreach (var job in jobs)
                {
                    job.Office.Name = ctx.Office.Where(s => s.Id == job.OfficeId).Select(s => s.Name).FirstOrDefault();
                    job.Position.Name = ctx.Position.Where(s => s.Id == job.PositionId).Select(s => s.Name).FirstOrDefault();
                    job.SeniorityLevel.SeniorityLevel1 = ctx.SeniorityLevel.Where(s => s.Id == job.SeniorityLevelId).Select(s => s.SeniorityLevel1).FirstOrDefault();
                }
                return View(jobs);
            }           
        }

        // GET: Job/Details/5
        public ActionResult Details(int id)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    var job = ctx.Job.Where(s => s.Id == id).FirstOrDefault();                   
                    job.Office.Name = ctx.Office.Where(s => s.Id == job.OfficeId).Select(s => s.Name).FirstOrDefault();
                    job.Position.Name = ctx.Position.Where(s => s.Id == job.PositionId).Select(s => s.Name).FirstOrDefault();
                    job.SeniorityLevel.SeniorityLevel1 = ctx.SeniorityLevel.Where(s => s.Id == job.SeniorityLevelId).Select(s => s.SeniorityLevel1).FirstOrDefault();
                    job.User = ctx.User.Where(s => s.Id == job.CreatedBy).FirstOrDefault();
                    job.User1 = ctx.User.Where(s => s.Id == job.UpdatedBy).FirstOrDefault();

                    return View(job);
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // GET: Job/Create
        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.positions = Positions();
                ViewBag.levels = Levels();
                ViewBag.offices = Offices();
                return View();
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // POST: Job/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Job job)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            var userName = User.Identity.GetUserName();
                            job.CreatedAt = DateTime.Now;
                            job.CreatedBy = ctx.User.Where(u => u.Email == userName).Select(u => u.Id).FirstOrDefault();
                            ctx.Job.Add(job);
                            ctx.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.positions = Positions();
                            ViewBag.levels = Levels();
                            ViewBag.offices = Offices();
                            return View(job);
                        }
                    }
                    catch
                    {
                        ViewBag.positions = Positions();
                        ViewBag.levels = Levels();
                        ViewBag.offices = Offices();
                        return View(job);
                    }
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // GET: Job/Edit/5
        public ActionResult Edit(int id)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    ViewBag.positions = Positions();
                    ViewBag.levels = Levels();
                    ViewBag.offices = Offices();
                    var job = ctx.Job.Where(s => s.Id == id).FirstOrDefault();
                    job.Office.Name = ctx.Office.Where(s => s.Id == job.OfficeId).Select(s => s.Name).FirstOrDefault();
                    job.Position.Name = ctx.Position.Where(s => s.Id == job.PositionId).Select(s => s.Name).FirstOrDefault();
                    job.SeniorityLevel.SeniorityLevel1 = ctx.SeniorityLevel.Where(s => s.Id == job.SeniorityLevelId).Select(s => s.SeniorityLevel1).FirstOrDefault();

                    return View(job);
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // POST: Job/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Job job)
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.positions = Positions();
                ViewBag.levels = Levels();
                ViewBag.offices = Offices();

                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            var userName = User.Identity.GetUserName();
                            job.UpdatedAt = DateTime.Now;
                            job.UpdatedBy = ctx.User.Where(u => u.Email == userName).Select(u => u.Id).FirstOrDefault();
                            ctx.Entry(job).State = System.Data.Entity.EntityState.Modified;
                            ctx.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            job.Office.Name = ctx.Office.Where(s => s.Id == job.OfficeId).Select(s => s.Name).FirstOrDefault();
                            job.Position.Name = ctx.Position.Where(s => s.Id == job.PositionId).Select(s => s.Name).FirstOrDefault();
                            job.SeniorityLevel.SeniorityLevel1 = ctx.SeniorityLevel.Where(s => s.Id == job.SeniorityLevelId).Select(s => s.SeniorityLevel1).FirstOrDefault();
                            return View(job);
                        }                        
                    }
                    catch (Exception e)
                    {                        
                       return View(job);
                    }
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        public List<SelectListItem> Positions()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
            {
                var positions = ctx.Position.Where(c => c.IsActive).ToList();

                foreach (var sub in positions)
                {
                    selectListItems.Add(new SelectListItem
                    {
                        Text = sub.Name,
                        Value = sub.Id.ToString()

                    });
                }
                return selectListItems;
            }
        }

        public List<SelectListItem> Offices()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
            {
                var offices = ctx.Office.Where(c => c.IsActive).ToList();

                foreach (var sub in offices)
                {
                    selectListItems.Add(new SelectListItem
                    {
                        Text = sub.Name,
                        Value = sub.Id.ToString()

                    });
                }
                return selectListItems;
            }
        }

        public List<SelectListItem> Levels()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
            {
                var levels = ctx.SeniorityLevel.ToList();

                foreach (var sub in levels)
                {
                    selectListItems.Add(new SelectListItem
                    {
                        Text = sub.SeniorityLevel1,
                        Value = sub.Id.ToString()

                    });
                }
                return selectListItems;
            }
        }
    }
}
