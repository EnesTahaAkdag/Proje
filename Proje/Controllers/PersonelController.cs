using Proje.Models.EntitiyFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Proje.ViewModels;
using System.Drawing.Printing;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Contexts;
using Microsoft.Ajax.Utilities;
using System.Data.Entity.Core.Metadata.Edm;

namespace Proje.Controllers
{
    [Authorize]

    public class PersonelController : Controller
    {
        PersonelDBEntities db = new PersonelDBEntities();

        public ActionResult Index()
        {
            var model = db.Personel.ToList();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Yeni()
        {
            var model = new PersonelAddViewModel();
            ViewBag.Departmanlar = db.Departman.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList(); ;
            return View("PersonelForm", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Yeni(PersonelAddViewModel model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departmanlar = db.Departman.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();
                return View("PersonelForm", model);
            }

            if (Request.Files.Count > 0)
            {
                string dosyaAdi = Path.GetFileName(Request.Files[0].FileName);
                string uzanti = Path.GetExtension(Request.Files[0].FileName);
                string yol = "~/Images/" + dosyaAdi + uzanti;
                Request.Files[0].SaveAs(Server.MapPath(yol));
                model.FileName = "~/Images/" + dosyaAdi + uzanti;
            }

            var existingImage = db.Personel.FirstOrDefault(p => p.FileName == model.FileName);

            var personel = new Personel()
            {
                DepartmanId = model.DepartmanId,
                Name = model.Name,
                SurName = model.SurName,
                Wage = model.Wage,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                Married = model.Married,
                FileName = model.FileName,
            };
            db.Personel.Add(personel);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Guncelle(int id)
        {
            var model = db.Personel.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.Departmanlar = db.Departman.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList(); 
            var editModel = new PersonelEditViewModel
            {
                DepartmanId = model.DepartmanId,
                Name = model.Name,
                SurName = model.SurName,
                Wage = (decimal)model.Wage,
                BirthDate = (DateTime)model.BirthDate,
                Gender = (bool)model.Gender,
                Married = model.Married,
                FileName = model.FileName,
            };
            return View("PersonelGuncelle", editModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guncelle(PersonelEditViewModel model, HttpPostedFileBase file, Personel personel)
        {
            //Resim Eklendimi Kontrol eder
            if (Request.Files.Count == 1 && Request.Files[0] != null)
            {
                var uploadedFile = Request.Files[0];
                string dosyaAdi = Path.GetFileName(uploadedFile.FileName);
                string uzanti = Path.GetExtension(uploadedFile.FileName);
                string yol = "~/Images/" + dosyaAdi + uzanti;
                uploadedFile.SaveAs(Server.MapPath(yol));
                model.FileName = "~/Images/" + dosyaAdi + uzanti;
            }
            if (!ModelState.IsValid)
            {
                return View("PersonelGuncelle", model);
            }

            var PersonelGuncelle = db.Personel.Find(model.Id);

          

            if (PersonelGuncelle == null)
            {
                return HttpNotFound();
            }

            PersonelGuncelle.Name = model.Name;
            PersonelGuncelle.SurName = model.SurName;
            PersonelGuncelle.DepartmanId = model.DepartmanId;
            PersonelGuncelle.Wage = model.Wage;
            PersonelGuncelle.BirthDate = model.BirthDate;
            PersonelGuncelle.Gender = model.Gender;
            PersonelGuncelle.Married = model.Married;
            PersonelGuncelle.FileName = model.FileName;

            db.Entry(PersonelGuncelle).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult PersonelDetay(int id)
        {
            var personel = db.Personel.Find(id);

            if (personel == null)
            {
                return HttpNotFound();
            }

            string imagePath = personel.FileName;

            return RedirectToAction("Index", imagePath);
        }
        [HttpPost]
        public ActionResult Sil(int personelId)
        {
            var silinecekPersonel = db.Personel.Find(personelId);
            if (silinecekPersonel == null)
                return HttpNotFound();
            db.Personel.Remove(silinecekPersonel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult PersonelleriListele(int id)
        {
            var model = db.Personel.Where(x => x.DepartmanId == id).ToList();
            return PartialView(model);
        }
        public ActionResult ToplamMaas()
        {
            ViewBag.Maas = db.Personel.Sum(x => x.Wage);
            return PartialView();
        }
    }
}