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
            return View("DepartmanForm", new Departman());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(Departman departman)
        {
            if (!ModelState.IsValid)
            { }
            

            var departmanAdiVarmi = db.Departman.FirstOrDefault(d => d.Name == departman.Name);
            if (departmanAdiVarmi != null)
            { 
            ModelState.AddModelError("Name", "Bu isimde bir departman zaten mevcut.");
            return View("DepartmanForm",departman);
            }

            if (departman.Id == 0)
            {
                db.Departman.Add(departman);
            }
            else
            {
                var guncellenecekDepartman = db.Departman.Find(departman.Id);
                if (guncellenecekDepartman == null)
                {
                    return HttpNotFound();
                }
                guncellenecekDepartman.Name = departman.Name;
            }

            db.SaveChanges();

            MesajVievModelController model = new MesajVievModelController();
            model.Status = true;
            model.Mesaj = departman.Name + (departman.Id == 0 ? " Departman Adı Eklendi " : " Departman Adı Güncellendi ");
            model.LinkText = "Departman Listesi";
            model.Url = "/Departman/Index";

            return View("_Mesaj", model);
        }
        public ActionResult Guncelle(int id)
        {

            var model = db.Departman.Find(id);
            if (model == null)
                return HttpNotFound();
            return View("DepartmanForm", model);
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