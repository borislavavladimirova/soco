using SoCoWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoCoWebApp.Controllers
{
    public class SeniorityLevelController : Controller
    {
        // GET: SeniorityLevel
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    return View(ctx.SeniorityLevel.ToList());
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // GET: SeniorityLevel/Create
        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // POST: SeniorityLevel/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SeniorityLevel level)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            ctx.SeniorityLevel.Add(level);
                            ctx.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        return View(level);
                    }
                    catch
                    {
                        return View(level);
                    }
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // GET: SeniorityLevel/Edit/5
        public ActionResult Edit(int id)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    return View(ctx.SeniorityLevel.Where(s => s.Id == id).FirstOrDefault());
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // POST: SeniorityLevel/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SeniorityLevel level)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            ctx.Entry(level).State = System.Data.Entity.EntityState.Modified;
                            ctx.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        return View(level);
                    }
                    catch
                    {
                        return View(level);
                    }
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // GET: SeniorityLevel/Delete/5
        public ActionResult Delete(int id)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    return View(ctx.SeniorityLevel.Where(s => s.Id == id).FirstOrDefault());
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // POST: SeniorityLevel/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, SeniorityLevel level)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            ctx.Entry(level).State = EntityState.Deleted;
                            ctx.SaveChanges();

                            return RedirectToAction("Index");
                        }
                        return View(level);
                    }
                    catch
                    {
                        return View(level);
                    }
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }
    }
}
