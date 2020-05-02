using KurumsalWeb.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using KurumsalWeb.Models.Model;

namespace KurumsalWeb.Controllers
{
    public class HomeController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        [Route("")]
        [Route("Anasayfa")]
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.Iletisim = db.Iletisim.SingleOrDefault();
            ViewBag.Hizmetler = db.Hizmet.ToList().OrderByDescending(a => a.HizmetId);
            ViewBag.Blog = db.Blog.ToList().OrderByDescending(a => a.BlogId);
            return View();
        }
        public ActionResult SliderPartial()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return PartialView(db.Slider.ToList().OrderByDescending(a => a.SliderId));
        }
        [Route("SliderPost/{baslik}/{id:int}")]
        public ActionResult SliderDetay(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var detay = db.Slider.Where(a => a.SliderId == id).SingleOrDefault();
            return View(detay);
        }
        public ActionResult FooterPartial()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.Iletisim = db.Iletisim.SingleOrDefault();
            return PartialView();
        }
        public ActionResult EnSonYapilanBlog()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return PartialView(db.Blog.ToList().OrderByDescending(a => a.BlogId));
        }       
        public ActionResult HizmetPartial()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return PartialView(db.Hizmet.ToList().OrderByDescending(a => a.HizmetId));
        }
        [Route("Hizmetler")]
        public ActionResult Hizmetler()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hizmet.ToList().OrderByDescending(a => a.HizmetId));
        }
        [Route("Hakkimizda")]
        public ActionResult Hakkimizda()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Hakkimizda.SingleOrDefault());
        }
        [Route("Iletisim")]
        public ActionResult Iletisim()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Iletisim.SingleOrDefault());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Iletisim(string adsoyad = null, string email = null, string konu = null, string mesaj = null)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (adsoyad != null && email != null)
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "baranvelat021@gmail.com";
                WebMail.Password = "bilgisayar21";
                WebMail.SmtpPort = 587;
                WebMail.Send("baranvelat021@gmail.com", konu, email + " - " + mesaj);
                ViewBag.Uyari = "Mesajınız başarıyla gönderilmiştir.";
                Response.Redirect("/Iletisim");
            }
            else
            {
                ViewBag.Uyari = "Mesaj gönderilirken hata oluştu.";
            }
            return View();
        }
        public ActionResult IletisimPartial()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.Iletisim = db.Iletisim.SingleOrDefault();
            return PartialView();
        }
        [Route("BlogPost")]
        public ActionResult Blog(int sayfa = 1)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Blog.Include("Yorums").Include("Kategori").ToList().OrderByDescending(a => a.BlogId).ToPagedList(sayfa, 4));
        }
        public ActionResult BlogKategoriPartial()
        {
            var blogkategori = db.Kategori.Include("Blogs").ToList().OrderByDescending(a => a.KategoriAd);
            return PartialView(blogkategori);
        }
        [Route("BlogPost/{baslik}-{id:int}")]
        public ActionResult BlogDetay(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Blog.Include("Yorums").Include("Kategori").Where(a => a.BlogId == id).SingleOrDefault());
        }
        public ActionResult EnSonGonderilerPartial()
        {
            return PartialView(db.Blog.ToList().OrderByDescending(a => a.BlogId));
        }
        [Route("BlogPost/{kategoriad}/{id:int}")]
        public ActionResult BlogKategori(int? id, int sayfa = 1)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Blog.Include("Yorums").Include("Kategori").Where(a => a.Kategori.KategoriId == id).OrderByDescending(a => a.BlogId).ToPagedList(sayfa, 4));
        }
        public JsonResult YorumYap(string adsoyad, string eposta, string icerik, int blogid)
        {
            if (icerik == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            db.Yorum.Add(new Yorum { AdSoyad = adsoyad, Eposta = eposta, Icerik = icerik, BlogId = blogid, Onay = false ,YorumTarih=DateTime.Now});
            db.SaveChanges();

            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult HizmetAlanPartial()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return PartialView(db.HizmetAlan.ToList());
        }
        [Route("HizmetAlan/{baslik}-{id:int}")]
        public ActionResult HizmetAlanDetay(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            var d = db.HizmetAlan.Where(a => a.HizmetAlanId == id).SingleOrDefault();
            return View(d);
        }
        public ActionResult HizmetAlanBaslik()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return PartialView(db.HizmetAlan.ToList());
        }
    }
}