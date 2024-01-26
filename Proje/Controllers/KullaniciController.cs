using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Proje.Models.EntitiyFramework;

namespace Proje.Controllers
{
    public class KullaniciController : Controller
    {
        private PersonelDBEntities db = new PersonelDBEntities();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Kullanici.ToList());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Kullanıcı adına göre sorgu yapın
            Kullanici kullanici = db.Kullanici.Where(k => k.Id == id).FirstOrDefault();

            if (kullanici == null)
            {
                return HttpNotFound();
            }

            return View(kullanici);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            var model = new Kullanici();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Password,Role")] Kullanici kullanici)
        {
            var kullaniciAdiVarmi = db.Kullanici.FirstOrDefault(k => k.Name == kullanici.Name);
            if (kullaniciAdiVarmi != null)
            {
                ModelState.AddModelError("Name", "Bu isimde bir Kullanıcı zaten mevcut.");
                return View(kullanici);
            }
            if (ModelState.IsValid)
            {
                db.Kullanici.Add(kullanici);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kullanici);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kullanici kullanici = db.Kullanici.Find(id);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Password,Role")] Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kullanici).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kullanici);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kullanici kullanici = db.Kullanici.Find(id);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Kullanici kullanici = db.Kullanici.Find(id);
            db.Kullanici.Remove(kullanici);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpGet]
        public ActionResult Profiles(string name)
        {
            if (name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Kullanıcı adına göre sorgu yapın
            Kullanici kullanici = db.Kullanici.Where(k => k.Name == name).FirstOrDefault();

            if (kullanici == null)
            {
                return HttpNotFound();
            }

            return View(kullanici);
        }
    }
}
