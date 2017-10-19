
using GaleriUygulamasi.Models;
using GaleriUygulamasi.Models.Context;
using GaleriUygulamasi.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GaleriUygulamasi.Controllers
{
    public class HomeController : Controller
    {
        GaleriYonetimContext _galerYonetimContext;
        public HomeController()
        {
            _galerYonetimContext = new GaleriYonetimContext();
        }
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Upload()
        {
            if (Session["value"] != null)
            {
                Response.Redirect("/");
            }
            return View();
        }

        public ActionResult FileUpload()
        {
            //Upload'dan gelen dtanin tipi HttpPostedFileBase olacak.
            HttpPostedFileBase file = HttpContext.Request.Files[0];

            //10KB şeklinde gelen verileri birleştirip veritabanına byte tipinde yazacağız
            using (BinaryReader reader = new BinaryReader(file.InputStream))
            {
                byte[] value = reader.ReadBytes((Int32)file.ContentLength);
                if (Session["value"] == null)
                {
                    Session["value"] = value;
                }
                else
                {
                    Session["value"] = UtilityManager.ByteBirlestir((byte[])Session["value"], value);
                }
                if (10000 > file.ContentLength)
                {
                    _galerYonetimContext.Dosya.Add(new Dosya
                    {
                        Deger = (byte[])Session["value"],
                        DosyaAdi = file.FileName,
                        DosyaBoyutu = ((byte[])Session["value"]).Length,
                        DosyaTipi = file.ContentType,
                        Ikon = UtilityManager.setIcon(file.ContentType),
                        BoyutKisaltma = UtilityManager.BytesToString(((byte[])Session["value"]).Length),
                        KayitTarihi = DateTime.Now
                    });
                    _galerYonetimContext.SaveChanges();
                    Session["value"] = null;
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Galeri()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetFileDetailById(int Id)
        {
            var file = (from d in _galerYonetimContext.Dosya
                        where d.ID == Id
                        select new
                        {
                            d.BoyutKisaltma,
                            d.DosyaBoyutu,
                            d.DosyaAdi,
                            d.DosyaTipi,
                            UrlYolu = "/Home/FileView/" + d.ID + "/" + d.Baslik,
                            d.Baslik,
                            d.Aciklama

                        }).ToList();

            return Json(file[0], JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateFile(UpFile file)
        {
            try
            {
                var entity = _galerYonetimContext.Dosya.Where(x => x.ID == file.ID).FirstOrDefault();
                if (entity != null)
                {
                    entity.Baslik = file.Baslik;
                    entity.Aciklama = file.Aciklama;
                    _galerYonetimContext.Entry(entity).State = EntityState.Modified;
                    _galerYonetimContext.SaveChanges();
                }
                return Json("E", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("H", JsonRequestBehavior.AllowGet);
            }
        }

        public FileContentResult FileView(int id, string dosyaAdi)
        {
            var file = _galerYonetimContext.Dosya.Where(p => p.ID == id).FirstOrDefault();
            return new FileContentResult(file.Deger, file.DosyaTipi);
        }
    }
}