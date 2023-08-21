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

namespace Proje.Controllers
{
    [Authorize]

    public class PersonelController : Controller
    {
        PersonelDBEntities db = new PersonelDBEntities();
        // GET: Personel
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

            // Eğer dosya yükleme işlemi yapılacaksa:
            if (file != null && file.ContentLength > 0)
            {
                var uploadDir = "~/Images/PersonelResimleri";
                var imagePath = Path.Combine(Server.MapPath(uploadDir), Path.GetFileName(file.FileName));
                file.SaveAs(imagePath);
                model.FileName = Path.Combine(uploadDir, file.FileName);
            }

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
            var model = new PersonelViewModel();
            ViewBag.Departmanlar = db.Departman.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList(); ;

            return View("PersonelForm", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult guncelle(PersonelEditViewModel model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View("PersonelForm", model);
            }
            var personelToUpdate = db.Personel.Find(model.Id);
            if (personelToUpdate == null)
            {
                return HttpNotFound();
            }
            personelToUpdate.Name = model.Name;
            personelToUpdate.SurName = model.SurName;
            personelToUpdate.DepartmanId = model.DepartmanId;
            personelToUpdate.Wage = model.Wage;
            personelToUpdate.BirthDate = model.BirthDate;
            personelToUpdate.Gender = model.Gender;
            personelToUpdate.Married = model.Married;
            if (file != null && file.ContentLength > 0)
            {
                var uploadDir = "~/Images/PersonelResimleri";
                var imagePath = Path.Combine(Server.MapPath(uploadDir), Path.GetFileName(file.FileName));
                file.SaveAs(imagePath);
                personelToUpdate.FileName = Path.Combine(uploadDir, file.FileName);
            }
            db.Entry(personelToUpdate).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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