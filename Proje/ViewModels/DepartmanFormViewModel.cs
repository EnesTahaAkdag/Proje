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

        [Display(Name = "Departman Adı")]
        [Required(ErrorMessage = "Departman Adı zorunludur")]
        public string Name { get; set; }
        public Departman Departman { get; set; }
    }
}