using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class HizmetController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        [Route("yonetimpaneli/hizmetler")]
        // GET: Hizmet
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hizmet.ToList().OrderByDescending(a=>a.HizmetId));
        }
        public ActionResult HizmetEkle()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult HizmetEkle(Hizmet hizmet,HttpPostedFileBase ResimURL)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                if(ResimURL != null)
                {
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);
                    string hizimgname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(800, 600);
                    img.Save("~/Uploads/Hizmet/" + hizimgname);
                    hizmet.ResimURL = "/Uploads/Hizmet/" + hizimgname;
                }
                db.Hizmet.Add(hizmet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hizmet);
        }
        public ActionResult HizmetGuncelle(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var g = db.Hizmet.Where(a=>a.HizmetId == id).SingleOrDefault();
            return View(g);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult HizmetGuncelle(int? id , Hizmet hizmet , HttpPostedFileBase ResimURL)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var h = db.Hizmet.Where(a => a.HizmetId == id).SingleOrDefault();
                var resim = h.ResimURL;
                if(ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(h.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(h.ResimURL));
                    }
                    WebImage hizimgguncelle = new WebImage(ResimURL.InputStream);
                    FileInfo hizinfo = new FileInfo(ResimURL.FileName);
                    string hizimgname = Guid.NewGuid().ToString() + hizinfo.Extension;
                    hizimgguncelle.Resize(800, 600);
                    hizimgguncelle.Save("~/Uploads/Hizmet/" + hizimgname);
                    h.ResimURL = "/Uploads/Hizmet/" + hizimgname;
                    h.Baslik = hizmet.Baslik;
                    h.Aciklama = hizmet.Aciklama;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    h.ResimURL = resim;
                    h.Baslik = hizmet.Baslik;
                    h.Aciklama = hizmet.Aciklama;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            return View(hizmet);
        }       
        public ActionResult HizmetSil(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (id == null)
            {
                return HttpNotFound();
            }
            var hizsil = db.Hizmet.Where(a => a.HizmetId == id).SingleOrDefault();
            if(hizsil == null)
            {
                return HttpNotFound();
            }
            if (System.IO.File.Exists(Server.MapPath(hizsil.ResimURL)))
            {
                System.IO.File.Delete(Server.MapPath(hizsil.ResimURL));
            }
            db.Hizmet.Remove(hizsil);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}