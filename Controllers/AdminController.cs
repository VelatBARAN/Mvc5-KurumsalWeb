using KurumsalWeb.Models;
using System.Collections;
using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using KurumsalWeb.Models.DataContext;
using KurumsalWeb.Models.Model;
using System.Web.Helpers;
using System.IO;

namespace KurumsalWeb.Controllers
{
    public class AdminController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        [Route("yonetimpaneli")]
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.BlogCount = db.Blog.Count();
            ViewBag.KategoriCount = db.Kategori.Count();
            ViewBag.HizmetCount = db.Hizmet.Count();
            ViewBag.YorumCount = db.Yorum.Count();
            ViewBag.YorumOnay = db.Yorum.Where(a => a.Onay == false).Count();
            return View();
        }
        [Route("yonetimpaneli/adminler")]
        public ActionResult Adminler()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Admin.Include("Yetki").ToList().OrderByDescending(a => a.AdminId));
        }
        public ActionResult AdminEkle()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.YetkiId = new SelectList(db.Yetki, "YetkiId", "YetkiAd");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AdminEkle(Admin admin, HttpPostedFileBase ResimURL)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                if (ResimURL != null)
                {
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo ImgInform = new FileInfo(ResimURL.FileName);
                    string adminimgname = Guid.NewGuid().ToString() + ImgInform.Extension;
                    img.Resize(50, 50);
                    img.Save("~/Uploads/Admin/" + adminimgname);
                    admin.ResimURL = "/Uploads/Admin/" + adminimgname;
                }
                admin.Sifre = Crypto.Hash(admin.Sifre, "MD5");
                db.Admin.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Adminler");
            }
            return View(admin);
        }
        public ActionResult AdminSil(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (id == null)
            {
                return HttpNotFound();
            }
            var adminsil = db.Admin.Where(a => a.AdminId == id).SingleOrDefault();
            if (adminsil == null)
            {
                return HttpNotFound();
            }
            if (System.IO.File.Exists(Server.MapPath(adminsil.ResimURL)))
            {
                System.IO.File.Delete(Server.MapPath(adminsil.ResimURL));
            }
            db.Admin.Remove(adminsil);
            db.SaveChanges();
            return RedirectToAction("Adminler");
        }
        public ActionResult AdminGuncelle(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var guncelle = db.Admin.Where(a => a.AdminId == id).SingleOrDefault();
            ViewBag.YetkiId = new SelectList(db.Yetki, "YetkiId", "YetkiAd");
            return View(guncelle);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AdminGuncelle(int? id, Admin admin, HttpPostedFileBase ResimURL)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var guncelle = db.Admin.Where(a => a.AdminId == id).SingleOrDefault();
                var resim = guncelle.ResimURL;
                string sifre = guncelle.Sifre;
                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(guncelle.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(guncelle.ResimURL));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo ImgInform = new FileInfo(ResimURL.FileName);
                    string adminname = Guid.NewGuid().ToString() + ImgInform.Extension;
                    img.Resize(300, 200);
                    img.Save("~/Uploads/Admin/" + adminname);
                    guncelle.ResimURL = "/Uploads/Admin/" + adminname;
                    guncelle.Ad = admin.Ad;
                    guncelle.Soyad = admin.Soyad;
                    guncelle.Eposta = admin.Eposta;
                    if (admin.Sifre == sifre)
                    {
                        guncelle.Sifre = admin.Sifre;
                    }
                    else
                    {
                        guncelle.Sifre = Crypto.Hash(admin.Sifre, "MD5"); // bakılacak.
                    }
                    guncelle.YetkiId = admin.YetkiId;
                    db.SaveChanges();
                    return RedirectToAction("Adminler");
                }
                else
                {
                    guncelle.ResimURL = resim;
                    guncelle.Ad = admin.Ad;
                    guncelle.Soyad = admin.Soyad;
                    guncelle.Eposta = admin.Eposta;
                    if (admin.Sifre == sifre)
                    {
                        guncelle.Sifre = admin.Sifre;
                    }
                    else
                    {
                        guncelle.Sifre = Crypto.Hash(admin.Sifre, "MD5"); // bakılacak.
                    }
                    guncelle.YetkiId = admin.YetkiId;
                    db.SaveChanges();
                    return RedirectToAction("Adminler");
                }

            }
            return View(admin);
        }
        [Route("yonetimpaneli/giris")]
        public ActionResult Login()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            var login = db.Admin.Include("Yetki").Where(a => a.Eposta == admin.Eposta).SingleOrDefault();
            if (login.Eposta == admin.Eposta && login.Sifre == Crypto.Hash(admin.Sifre, "MD5"))
            {
                Session["adminid"] = login.AdminId;
                Session["ad"] = login.Ad;
                Session["soyad"] = login.Soyad;
                Session["eposta"] = login.Eposta;
                Session["yetki"] = login.Yetki.YetkiAd;
                Session["resim"] = login.ResimURL;
                return RedirectToAction("", "yonetimpaneli");
            }
            ViewBag.Uyari = "Kullanıcı adı ya da şifre hatalı...";
            return View(admin);
        }
        public ActionResult LogOut()
        {
            Session["adminid"] = null;
            Session["ad"] = null;
            Session["soyad"] = null;
            Session["eposta"] = null;
            Session["yetki"] = null;
            Session["resim"] = null;
            return RedirectToAction("", "yonetimpaneli/giris");
        }
        public ActionResult SifremiUnuttum()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SifremiUnuttum(Admin admin)
        {
            var sifreunuttum = db.Admin.Where(a => a.Eposta == admin.Eposta).SingleOrDefault();
            if (sifreunuttum != null)
            {
                Random rnd = new Random();
                int yenisayi = rnd.Next();
                sifreunuttum.Sifre = Crypto.Hash(Convert.ToString(yenisayi), "MD5");
                db.SaveChanges();
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "baranvelat021@gmail.com";
                WebMail.Password = "bilgisayar21";
                WebMail.SmtpPort = 587;
                WebMail.Send(admin.Eposta, "Admin Paneli Giriş Şifreniz ", ":" + yenisayi);
                ViewBag.Uyari = "Şifreniz başarıyla gönderilmiştir.";
            }
            else
            {
                ViewBag.Uyari = "Şifre gönderilirken hata oluştu...";
            }
            return View();
        }
    }
}