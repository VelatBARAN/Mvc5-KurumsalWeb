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
    public class YorumController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();

        [Route("yonetimpaneli/blogyorum")]
        // GET: Yorum
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var yorum = db.Yorum.Include(y => y.Blog);
            return View(yorum.ToList());
        }
        // GET: Yorum/Create
        public ActionResult Create()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.BlogId = new SelectList(db.Blog, "BlogId", "Baslik");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "YorumId,AdSoyad,Eposta,Icerik,Onay,YorumTarih,BlogId")] Yorum yorum)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                db.Yorum.Add(yorum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BlogId = new SelectList(db.Blog, "BlogId", "Baslik", yorum.BlogId);
            return View(yorum);
        }
        // GET: Yorum/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorum yorum = db.Yorum.Find(id);
            if (yorum == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogId = new SelectList(db.Blog, "BlogId", "Baslik", yorum.BlogId);
            return View(yorum);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "YorumId,AdSoyad,Eposta,Icerik,Onay,YorumTarih,BlogId")] Yorum yorum)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                db.Entry(yorum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogId = new SelectList(db.Blog, "BlogId", "Baslik", yorum.BlogId);
            return View(yorum);
        }
        // GET: Yorum/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorum yorum = db.Yorum.Find(id);
            if (yorum == null)
            {
                return HttpNotFound();
            }
            return View(yorum);
        }
        // POST: Yorum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            Yorum yorum = db.Yorum.Find(id);
            db.Yorum.Remove(yorum);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
