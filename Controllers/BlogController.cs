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
    public class BlogController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        [Route("yonetimpaneli/blogkayit")]
        // GET: Blog
        public ActionResult Index()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            return View(db.Blog.Include("Kategori").ToList().OrderByDescending(a=>a.BlogId));
        }
        public ActionResult BlogEkle()
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAd");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult BlogEkle(Blog blog , HttpPostedFileBase ResimURL)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                if(ResimURL != null)
                {
                    WebImage blogimg = new WebImage(ResimURL.InputStream);
                    FileInfo blogimginfo = new FileInfo(ResimURL.FileName);
                    string blogimgname = Guid.NewGuid().ToString() + blogimginfo.Extension;
                    blogimg.Resize(800, 450);
                    blogimg.Save("~/Uploads/Blog/" + blogimgname);
                    blog.ResimURL = "/Uploads/Blog/" + blogimgname;
                }
                db.Blog.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");   
            }
            return View();
        }
        public ActionResult BlogGuncelle(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            ViewBag.KategoriId = new SelectList(db.Kategori, "KategoriId", "KategoriAd");
            var g = db.Blog.Where(a => a.BlogId == id).SingleOrDefault();
            return View(g);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult BlogGuncelle(int? id , Blog blog , HttpPostedFileBase ResimURL)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (ModelState.IsValid)
            {
                var guncelle = db.Blog.Where(a => a.BlogId == id).SingleOrDefault();
                var resm = guncelle.ResimURL;
                if (ResimURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(guncelle.ResimURL)))
                    {
                        System.IO.File.Delete(Server.MapPath(guncelle.ResimURL));
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo ImgInform = new FileInfo(ResimURL.FileName);
                    string blogimgname = Guid.NewGuid().ToString() + ImgInform.Extension;
                    img.Resize(800, 450);
                    img.Save("~/Uploads/Blog/" + blogimgname);
                    guncelle.ResimURL = "/Uploads/Blog/" + blogimgname;
                    guncelle.Baslik = blog.Baslik;
                    guncelle.Icerik = blog.Icerik;
                    guncelle.KategoriId = blog.KategoriId;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    guncelle.ResimURL = resm;
                    guncelle.Baslik = blog.Baslik;
                    guncelle.Icerik = blog.Icerik;
                    guncelle.KategoriId = blog.KategoriId;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
                return View(blog);
        }
        public ActionResult BlogSil(int? id)
        {
            ViewBag.Kimlik = db.Kimlik.SingleOrDefault();
            if (id == null)
            {
                return HttpNotFound();
            }
            var blogsil = db.Blog.Where(a => a.BlogId == id).SingleOrDefault();
            if(blogsil == null)
            {
                return HttpNotFound();
            }
            if (System.IO.File.Exists(Server.MapPath(blogsil.ResimURL)))
            {
                System.IO.File.Delete(Server.MapPath(blogsil.ResimURL));
            }
            db.Blog.Remove(blogsil);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}