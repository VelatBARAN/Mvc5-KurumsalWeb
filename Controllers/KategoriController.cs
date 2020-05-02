using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class KategoriController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        [Route("yonetimpaneli/blogkategori")]
        // GET: Kategori
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Kategori.ToList().OrderByDescending(a=>a.KategoriId));
        }
        public ActionResult KategoriEkle()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult KategoriEkle(Kategori kategori)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                db.Kategori.Add(kategori);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kategori);
        }
        public ActionResult KategoriGuncelle(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (id == null)
            {
                return HttpNotFound();
            }
            var g = db.Kategori.Where(a => a.KategoriId == id).SingleOrDefault();
            return View(g);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult KategoriGuncelle(int? id, Kategori kategori)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                    var ktgrguncelle = db.Kategori.Where(a => a.KategoriId == id).SingleOrDefault();
                    if (ktgrguncelle == null)
                    {
                        return HttpNotFound();
                    }
                    ktgrguncelle.KategoriAd = kategori.KategoriAd;
                    ktgrguncelle.Aciklama = kategori.Aciklama;
                    db.SaveChanges();
                    return RedirectToAction("Index");
            }
            return View(kategori);
        }
        public ActionResult KategoriSil(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (id == null)
            {
                return HttpNotFound();
            }
            var ktgrsil = db.Kategori.Where(a => a.KategoriId == id).SingleOrDefault();
            if(ktgrsil == null)
            {
                return HttpNotFound();
            }
            db.Kategori.Remove(ktgrsil);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}