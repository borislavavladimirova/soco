using SoCoWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoCoWebApp.Controllers
{
    public class OfficeController : Controller
    {
        // GET: Office
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    var offices = ctx.Office.ToList();
                    foreach (var office in offices)
                    {
                        office.Country.Name = ctx.Country.Where(s => s.Id == office.CountryId).Select(s => s.Name).FirstOrDefault();
                    }
                    return View(offices);
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // GET: Office/Create
        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.countries = Countries();
                return View();
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // POST: Office/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Office office)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            ctx.Office.Add(office);
                            ctx.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        ViewBag.countries = Countries();
                        return View(office);
                    }
                    catch
                    {
                        ViewBag.countries = Countries();
                        return View(office);
                    }
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // GET: Office/Edit/5
        public ActionResult Edit(int id)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    ViewBag.countries = Countries();
                    var office = ctx.Office.Where(s => s.Id == id).FirstOrDefault();
                    office.Country.Name = ctx.Country.Where(s => s.Id == office.CountryId).Select(s => s.Name).FirstOrDefault();
                    return View(office);
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // POST: Office/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Office office)
        {
            if (Request.IsAuthenticated)
            {
                using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            ctx.Entry(office).State = System.Data.Entity.EntityState.Modified;
                            ctx.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        ViewBag.countries = Countries();
                        office.Country.Name = ctx.Country.Where(s => s.Id == office.CountryId).Select(s => s.Name).FirstOrDefault();
                        return View(office);
                    }
                    catch
                    {
                        ViewBag.countries = Countries();
                        office.Country.Name = ctx.Country.Where(s => s.Id == office.CountryId).Select(s => s.Name).FirstOrDefault();
                        return View(office);
                    }
                }
            }
            else return RedirectToAction("Login", "User", new { returnUrl = Request.Url.AbsolutePath });
        }

        // GET: Active Offices
        public ActionResult ActiveOffices()
        {           
            using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
            {
                var offices = ctx.Office.Where(o => o.IsActive).ToList();
                foreach (var office in offices)
                {
                    office.Country.Name = ctx.Country.Where(s => s.Id == office.CountryId).Select(s => s.Name).FirstOrDefault();
                }
                return View(offices);
            }           
        }

        public List<SelectListItem> Countries()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            using (SoftwareCompanyDatabaseEntities ctx = new SoftwareCompanyDatabaseEntities())
            {
                var countries = ctx.Country.Where(c => c.IsActive).ToList();

                foreach (var sub in countries)
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
    }
}
