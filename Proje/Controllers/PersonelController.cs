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
            var model = new Class1()
            {
                Departmanlar = db.Departman.ToList(),
                Personel = new Personel()
            };
            return View("PersonelForm", model);
        }
        [ValidateAntiForgeryToken]
        public ActionResult Kaydet(Personel personel)
        {
            if (!ModelState.IsValid)
            {
                var model = new Class1()
                {
                    Departmanlar = db.Departman.ToList(),
                    Personel = personel
                };
                return View("PersonelForm", model);
            }
            if (personel.Id == 0)
            {
                db.Personel.Add(personel);
            }
            else
            {
                db.Entry(personel).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Guncelle(int id)
        {
            var model = new Class1()
            {
                Departmanlar = db.Departman.ToList(),
                Personel = db.Personel.Find(id)
            };
            return View("PersonelForm", model);
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