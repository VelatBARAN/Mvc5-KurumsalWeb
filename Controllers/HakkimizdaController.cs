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
    public class HakkimizdaController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        [Route("yonetimpaneli/hakkimizda")]
        // GET: Hakkimizda
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hakkimizda.ToList());
        }
        public ActionResult Guncelle(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var g = db.Hakkimizda.Where(a => a.HakkimizdaId == id).SingleOrDefault();
            return View(g);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Guncelle(int? id, Hakkimizda hk, HttpPostedFileBase ResimURL)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var g = db.Hakkimizda.Where(a => a.HakkimizdaId == id).SingleOrDefault();
                var resim = g.ResimURL;
                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(g.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(g.ResimURL));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName);
                    string hkimgname = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Resize(500, 500);
                    img.Save("~/Uploads/Hakkimizda/" + hkimgname);
                    g.ResimURL = "/Uploads/Hakkimizda/" + hkimgname;
                    g.Aciklama = hk.Aciklama;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    g.ResimURL = resim;
                    g.Aciklama = hk.Aciklama;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }            
            return View(hk);
        }
    }
}