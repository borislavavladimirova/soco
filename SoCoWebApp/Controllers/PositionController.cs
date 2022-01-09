using SoCoWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoCoWebApp.Controllers
{
    public class PositionController : Controller
    {
        // GET: Position
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    return View(ctx.Position.ToList());
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }
      
        // GET: Position/Create
        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // POST: Position/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Position position)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            ctx.Position.Add(position);
                            ctx.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        return View(position);
                    }
                    catch
                    {
                        return View(position);
                    }
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // GET: Position/Edit/5
        public ActionResult Edit(int id)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    return View(ctx.Position.Where(s => s.Id == id).FirstOrDefault());
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // POST: Position/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Position position)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            ctx.Entry(position).State = System.Data.Entity.EntityState.Modified;
                            ctx.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        return View(position);
                    }
                    catch
                    {
                        return View(position);
                    }
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }
    }
}
