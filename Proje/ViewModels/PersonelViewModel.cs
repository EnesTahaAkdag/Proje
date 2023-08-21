using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proje.ViewModels
{
    public class PersonelViewModel
    {
        public long? DepartmanId { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public decimal Wage { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Gender { get; set; }
        public bool Married { get; set; }
        public string FileName { get; set; }
    }

    public class PersonelAddViewModel : PersonelViewModel
    {

    }

    public class PersonelEditViewModel : PersonelViewModel
    {
        public int Id { get; set; }
    }
}