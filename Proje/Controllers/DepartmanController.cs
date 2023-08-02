using Microsoft.Ajax.Utilities;
using Proje.Models.EntitiyFramework;
using Proje.ViewModels;
using System;
using System.Collections.Generic;
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
            MesajVievModelController model = new MesajVievModelController();
            if (!ModelState.IsValid)
            {
                return View("DepartmanForm");
            }
            if (departman.Id == 0)
            {
                db.Departman.Add(departman);
                model.Mesaj = departman.Name + " " + " Departman Adı Eklendi " + " ";
            }
            else
            {
                var guncellenecekDepartman = db.Departman.Find(departman.Id);
                if (guncellenecekDepartman == null)
                {
                    return HttpNotFound();
                }
                guncellenecekDepartman.Name = departman.Name;
                model.Mesaj = departman.Name + " " + "Departman Adı Eklendi" + " ";
            }
            db.SaveChanges();
            model.Status = true;
            model.LinkText = "Departman Listesi";
            model.Url = "Departman";
            return View("_Mesaj", model);
        }
        public ActionResult Guncelle(int id)
        {
            var model = db.Departman.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View("DepartmanForm", model);
        }

        public ActionResult Sil(int id)
        {
            var silinecekDepartman = db.Departman.Find(id);
            if (silinecekDepartman == null)
            {
                return HttpNotFound();
            }
            if (db.Personel.FirstOrDefault(x => x.DepartmanId == id) == null)
            {
                db.Departman.Remove(silinecekDepartman);
                return RedirectToAction("Index");
            }
            else
            {
                return View("RecordInUse");
            }
            db.SaveChanges();
        }
    }
}