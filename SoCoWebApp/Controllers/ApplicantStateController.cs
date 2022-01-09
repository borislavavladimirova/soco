using SoCoWebApp.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoCoWebApp.Controllers
{
    public class ApplicantStateController : Controller
    {
        // GET: ApplicantState
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                   return View(ctx.ApplicantState.ToList());                
                }                  
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }
        
        // GET: ApplicantState/Create
        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {              
                return View();
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // POST: ApplicantState/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApplicantState applicantState)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {                   
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            ctx.ApplicantState.Add(applicantState);
                            ctx.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        return View(applicantState);
                    }
                    catch
                    {
                        return View(applicantState);
                    }
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // GET: ApplicantState/Edit/5
        public ActionResult Edit(int id)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {                   
                    return View(ctx.ApplicantState.Where(s => s.Id == id).FirstOrDefault());
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // POST: ApplicantState/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ApplicantState applicantState)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {                    
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            ctx.Entry(applicantState).State = System.Data.Entity.EntityState.Modified;
                            ctx.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        return View(applicantState);
                    }
                    catch
                    {
                        return View(applicantState);
                    }
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }
    }
}
