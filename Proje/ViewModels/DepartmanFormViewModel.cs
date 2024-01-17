using Proje.Models.EntitiyFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Proje.ViewModels
{
    public class DepartmanFormViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Departman Adı Boş Bırakılamaz")]
        [StringLength(50, ErrorMessage = "50 Karakterden Fazla Karakter Girişi Yapılamaz")]
        public string Name { get; set; }
        public Departman Departman { get; set; }
    }
    public class DepartmanEditViewModel 
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Departman Adı Boş Bırakılamaz")]
        [StringLength(50, ErrorMessage = "50 Karakterden Fazla Karakter Girişi Yapılamaz")]
        public string Name { get; set; }
        public Departman Departman { get; set; }
    }
}