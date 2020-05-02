using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;

namespace KurumsalWeb.Controllers
{
    public class GorselController : Controller
    {
        private KurumsalDBContext db = new KurumsalDBContext();

        [Route("yonetimpaneli/gorseller")]
        // GET: Gorsel
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Slider.ToList().OrderByDescending(a=>a.SliderId));
        }

        public ActionResult Create()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Slider slider, HttpPostedFileBase ResimURL)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                if (ResimURL != null)
                {
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);
                    string silimgname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(1024, 360);
                    img.Save("~/Uploads/Slider/" + silimgname);
                    slider.ResimURL = "/Uploads/Slider/" + silimgname;
                }
                db.Slider.Add(slider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var s = db.Slider.Where(a => a.SliderId == id).SingleOrDefault();
            return View(s);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int? id, Slider slider, HttpPostedFileBase ResimURL)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var s = db.Slider.Where(a => a.SliderId == id).SingleOrDefault();
                var resim = s.ResimURL;
                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(s.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(s.ResimURL));
                    }
                    WebImage sliderimgguncelle = new WebImage(ResimURL.InputStream);
                    FileInfo sliderinfo = new FileInfo(ResimURL.FileName);
                    string sliderimgname = Guid.NewGuid().ToString() + sliderinfo.Extension;
                    sliderimgguncelle.Resize(1024, 360);
                    sliderimgguncelle.Save("~/Uploads/Slider/" + sliderimgname);
                    s.ResimURL = "/Uploads/Slider/" + sliderimgname;
                    s.Baslik = slider.Baslik;
                    s.Aciklama = slider.Aciklama;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    s.ResimURL = resim;
                    s.Baslik = slider.Baslik;
                    s.Aciklama = slider.Aciklama;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

            return View(slider);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var s = db.Slider.Where(a => a.SliderId == id).SingleOrDefault();
            if (s == null)
            {
                return HttpNotFound();
            }
            db.Slider.Remove(s);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
