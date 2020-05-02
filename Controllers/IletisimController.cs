using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KurumsalWeb.Controllers
{
    public class IletisimController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        [Route("yonetimpaneli/iletisim")]
        // GET: Iletisim
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Iletisim.ToList().SingleOrDefault());
        }
        public ActionResult IletisimGuncelle(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var g = db.Iletisim.Where(a => a.IletisimId == id).SingleOrDefault();
            return View(g);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult IletisimGuncelle(int? id,Iletisim iletisim)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var guncelle = db.Iletisim.Where(a => a.IletisimId == id).SingleOrDefault();
                if(guncelle == null)
                {
                    return HttpNotFound();
                }
                guncelle.Adres = iletisim.Adres;
                guncelle.Telefon = iletisim.Telefon;
                guncelle.Fax = iletisim.Fax;
                guncelle.Telegram = iletisim.Telegram;
                guncelle.Linkedln = iletisim.Linkedln;
                guncelle.Twitter = iletisim.Twitter;
                guncelle.Instagram = iletisim.Instagram;
                guncelle.Email = iletisim.Email;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(iletisim);
        }
    }
}