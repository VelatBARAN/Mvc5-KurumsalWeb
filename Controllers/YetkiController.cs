using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;

namespace KurumsalWeb.Controllers
{
    public class YetkiController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();

        [Route("yonetimpaneli/yetki")]
        // GET: Yetki
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Yetki.ToList().OrderByDescending(a=>a.YetkiId));
        }
        // GET: Yetki/Create
        public ActionResult Create()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View();
        }
        // POST: Yetki/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "YetkiId,YetkiAd")] Yetki yetki)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                db.Yetki.Add(yetki);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(yetki);
        }
        // GET: Yetki/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yetki yetki = db.Yetki.Find(id);
            if (yetki == null)
            {
                return HttpNotFound();
            }
            return View(yetki);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "YetkiId,YetkiAd")] Yetki yetki)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                db.Entry(yetki).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yetki);
        }
        public ActionResult YetkiSil(int id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var sil = db.Yetki.Find(id);
            if(sil != null)
            {
                db.Yetki.Remove(sil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sil);
        }
    }
}
