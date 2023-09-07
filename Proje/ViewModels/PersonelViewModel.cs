using Proje.Models.EntitiyFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Proje.ViewModels
{
    public class PersonelViewModel
    {

        public long? DepartmanId { get; set; }

        [Required(ErrorMessage = "Personel Adı Boş Bırakılamaz")]
        [StringLength(50, ErrorMessage = "50 Karakterden Fazla Karakter Girişi Yapılamaz")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Personel Soyadı Boş Bırakılamaz")]
        [StringLength(50, ErrorMessage = "50 Karakterden Fazla Karakter Girişi Yapılamaz")]
        public string SurName { get; set; }

        [Required(ErrorMessage = "Personel Maaşı Boş Bırakılamaz")]
        public decimal Wage { get; set; }

        [Required(ErrorMessage = "Personel Doğum Tarihi Boş Bırakılamaz")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Personel Cinsiyeti Boş Bırakılamaz")]
        public bool Gender { get; set; }


        public bool Married { get; set; }
        public string FileName { get; set; }
        public IEnumerable<Departman>Departmanlar{ get; set; }
    }

    public class PersonelAddViewModel : PersonelViewModel
    {

    }

    public class PersonelEditViewModel : PersonelViewModel
    {
        public int Id { get; set; }
    }
}