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
    public class KimlikController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        [Route("yonetimpaneli/kimlik")]
        // GET: Kimlik
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Kimlik.ToList());
        }
        // GET: Kimlik/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var kimlik = db.Kimlik.Where(x => x.KimlikId == id).SingleOrDefault();
            return View(kimlik);
        }
        // POST: Kimlik/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Kimlik kimlik, HttpPostedFileBase ResimURL)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var k = db.Kimlik.Where(x => x.KimlikId == id).SingleOrDefault();
                var resim = k.ResimURL;

                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(k.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(k.ResimURL));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo ImgInform = new FileInfo(ResimURL.FileName);
                    string logoname =Guid.NewGuid().ToString() + ImgInform.Extension;
                    img.Resize(200, 200);
                    img.Save("~/Uploads/Kimlik/" + logoname);
                    k.ResimURL = "/Uploads/Kimlik/" + logoname;
                    k.Title = kimlik.Title;
                    k.Keywords = kimlik.Keywords;
                    k.Description = kimlik.Description;
                    k.Unvan = kimlik.Unvan;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    k.ResimURL = resim;
                    k.Title = kimlik.Title;
                    k.Keywords = kimlik.Keywords;
                    k.Description = kimlik.Description;
                    k.Unvan = kimlik.Unvan;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            return View(kimlik);
        }
    }
}
