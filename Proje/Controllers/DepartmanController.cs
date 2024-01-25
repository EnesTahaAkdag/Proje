using Microsoft.Ajax.Utilities;
using Proje.Models.EntitiyFramework;
using Proje.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;

namespace Proje.Controllers
{

    [Authorize]
    public class DepartmanController : Controller
    {
        PersonelDBEntities db = new PersonelDBEntities();
        public ActionResult Index()
        {
            var model = db.Departman.ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult DepartmanForm()
        {
            var model = new DepartmanFormViewModel();
            return View("DepartmanForm", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(DepartmanFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("DepartmanForm", model);
            }
            var yeniDepartman = new Departman()
            {
                Name = model.Name
            };
            db.Departman.Add(yeniDepartman);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Guncelle(int id)
        {
            var model = db.Departman.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }

            var editModel = new DepartmanEditViewModel
            {
                Id = id,
                Name = model.Name,
            };

            return View("Guncelle", editModel);
        }
        [HttpPost]
        public ActionResult Guncelle(DepartmanEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Model = db.Departman.FirstOrDefault(x => x.Id == model.Id);
                Model.Name = model.Name;
                db.SaveChanges();
                return RedirectToAction("Index", "Departman");
            }
            return RedirectToAction("Guncelle", "Personel");
        }
        [HttpPost]
        public ActionResult Personelvarmi(int departmanId)
        {
            //Departmanın İçerisindeki Personeli Kontrol Eder
            var personelVarmi = db.Personel.Any(x => x.DepartmanId == departmanId);
            return Json(personelVarmi);
        }
        [HttpPost]
        public ActionResult Sil(int departmanId, string type)
        {
            var silinecekDepartman = db.Departman.Find(departmanId);
            var personels = db.Personel.Where(x => x.DepartmanId == departmanId).ToList();
            //Boş Departmanı Sil
            if (type == "onlyDpt")
            {
                if (silinecekDepartman == null)
                    return HttpNotFound();
                db.Departman.Remove(silinecekDepartman);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //Departmanı Sil Personeli Boşa Çıkar
            else if (type == "removeDptemptyPersonel")
            {
                foreach (var item in personels)
                    item.DepartmanId = null;
                db.Departman.Remove(silinecekDepartman);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //Departman ve Personeli Birlikte Sil
            else if (type == "removeDptandPersonel")
            {
                if (personels == null || silinecekDepartman == null)
                    return HttpNotFound();
                foreach (var item in personels)
                {
                    db.Personel.Remove(item);
                }
                db.Departman.Remove(silinecekDepartman);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Departman", "Index");
        }
    }
}