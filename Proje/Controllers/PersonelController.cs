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
            //yeni eklenecek personel için personel kayıt penceresini açar
            var model = new PersonelAddViewModel();
            ViewBag.Departmanlar = db.Departman.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList(); ;
            return View("PersonelForm", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Yeni(PersonelAddViewModel Model, HttpPostedFileBase uploadFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departmanlar = db.Departman.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();
                return View("PersonelForm", Model);
            }
            //personelin resmini kaydeder
            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                var tempImageDirectory = Server.MapPath("~/Content/uploads/");
                var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(uploadFile.FileName)}";
                var pathImage = Path.Combine(tempImageDirectory, fileName);

                // Eğer yeni dosya yüklendi ise, eski dosyayı silerek yeni dosyayı kaydet
                if (!Directory.Exists(tempImageDirectory))
                    Directory.CreateDirectory(tempImageDirectory);

                uploadFile.SaveAs(pathImage);

                // Eski dosyayı sil
                if (!string.IsNullOrEmpty(Model.FileName))
                {
                    var oldFilePath = Path.Combine(tempImageDirectory, Model.FileName);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                Model.FileName = fileName;
            }

            var existingImage = db.Personel.FirstOrDefault(p => p.FileName == Model.FileName);
            //personelin verilerini kaydeder
            var personel = new Personel()
            {
                DepartmanId = Model.DepartmanId,
                Name = Model.Name,
                SurName = Model.SurName,
                Wage = Model.Wage,
                BirthDate = Model.BirthDate,
                Gender = Model.Gender,
                Married = Model.Married,
                FileName = Model.FileName,
            };
            db.Personel.Add(personel);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
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
                Id = id,
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
        public ActionResult Guncelle(PersonelEditViewModel model, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {
                var Model = db.Personel.FirstOrDefault(x => x.Id == model.Id);

                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    var tempImageDirectory = Server.MapPath("~/Content/uploads/");
                    var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(uploadFile.FileName)}";
                    var pathImage = Path.Combine(tempImageDirectory, fileName);

                    // Eğer yeni dosya yüklendi ise, eski dosyayı silerek yeni dosyayı kaydet
                    if (!Directory.Exists(tempImageDirectory))
                        Directory.CreateDirectory(tempImageDirectory);

                    uploadFile.SaveAs(pathImage);

                    // Eski dosyayı sil
                    if (!string.IsNullOrEmpty(Model.FileName))
                    {
                        var oldFilePath = Path.Combine(tempImageDirectory, Model.FileName);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    Model.FileName = fileName;
                }
                Model.Name = model.Name;
                Model.SurName = model.SurName;
                Model.DepartmanId = model.DepartmanId;
                Model.Wage = model.Wage;
                Model.BirthDate = model.BirthDate;
                Model.Gender = model.Gender;
                Model.Married = model.Married;
                db.SaveChanges();
                return RedirectToAction("Index", "Personel");
            }
            return RedirectToAction("PersonelGuncelle", "Personel");
        }
        public ActionResult PersonelDetay(int id)
        {
            //kişinin id sine göre resmi ekrana verir
            var personel = db.Personel.Find(id);

            if (personel == null)
            {
                return HttpNotFound();
            }

            string imagePath = personel.FileName;

            return RedirectToAction("Index", imagePath);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Sil(int personelId)
        {
            //personeli silme işlemi yapar
            var silinecekPersonel = db.Personel.Find(personelId);
            if (silinecekPersonel == null)
                return HttpNotFound();
            db.Personel.Remove(silinecekPersonel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult PersonelleriListele(int id)
        {
            //personeli listeleme işlemi yapar
            var model = db.Personel.Where(x => x.DepartmanId == id).ToList();
            return PartialView(model);
        }
        public ActionResult ToplamMaas()
        {
            //personelin maaşını ekranada gösterir
            ViewBag.Maas = db.Personel.Sum(x => x.Wage);
            return PartialView();
        }
    }
}