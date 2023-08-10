using Proje.Models.EntitiyFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Proje.Controllers
{
    [AllowAnonymous]
    public class SecurityController : Controller
    {
     
        PersonelDBEntities db = new PersonelDBEntities();
        // GET: Security
        public ActionResult Login()
        {
            var model = new Kullanici();
            return View(model);
        }
        [HttpPost]
        public ActionResult Login(Kullanici kullanici)
        {
            var kullaniciIndb = db.Kullanici.FirstOrDefault(x => x.Name == kullanici.Name && x.Password == kullanici.Password);
            if (kullaniciIndb != null)
            {
                FormsAuthentication.SetAuthCookie(kullaniciIndb.Name, true);
                return RedirectToAction("Index", "Departman");
            }
            else
            {
                ViewBag.Mesaj = "geçersinz kullanıcı Adı veya Şifre";
                return View();
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}