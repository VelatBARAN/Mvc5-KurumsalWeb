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
    public class HizmetAlanController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        [Route("yonetimpaneli/hizmetalanlar")]
        // GET: HizmetAlan
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.HizmetAlan.ToList());
        }
        public ActionResult HizmetAlanEkle()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult HizmetAlanEkle(HizmetAlan hizmetalan,HttpPostedFileBase ResimURL)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                if (ResimURL != null)
                {
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);
                    string imgname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(300, 300);
                    img.Save("~/Uploads/HizmetAlan/" + imgname);
                    hizmetalan.ResimURL = "/Uploads/HizmetAlan/" + imgname;
                }
                db.HizmetAlan.Add(hizmetalan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hizmetalan);
        }
        public ActionResult HizmetAlanGuncelle(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var g = db.HizmetAlan.Where(a => a.HizmetAlanId == id).SingleOrDefault();
            return View(g);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult HizmetAlanGuncelle(int? id,HizmetAlan hizmetalan, HttpPostedFileBase ResimURL)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var guncelle = db.HizmetAlan.Where(a => a.HizmetAlanId == id).SingleOrDefault();
                var resm = guncelle.ResimURL;
                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(guncelle.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(guncelle.ResimURL));
                    }
                    else
                    {
                        WebImage img = new WebImage(ResimURL.InputStream);
                        FileInfo ImgInform = new FileInfo(ResimURL.FileName);
                        string imgname = Guid.NewGuid().ToString() + ImgInform.Extension;
                        img.Resize(300, 300);
                        img.Save("~/Uploads/HizmetAlan/" + imgname);
                        guncelle.ResimURL = "/Uploads/HizmetAlan/" + imgname;
                        guncelle.Baslik = hizmetalan.Baslik;
                        guncelle.Aciklama = hizmetalan.Aciklama;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    guncelle.ResimURL = resm;
                    guncelle.Baslik = hizmetalan.Baslik;
                    guncelle.Aciklama = hizmetalan.Aciklama;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(hizmetalan);
        }
        public ActionResult HizmetAlanSil(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var s = db.HizmetAlan.Where(a => a.HizmetAlanId == id).SingleOrDefault();
            if (id == null) { return HttpNotFound(); }          
            else if(s == null) { return HttpNotFound(); }
            db.HizmetAlan.Remove(s);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}