using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proje.ViewModels
{
    public class MesajVievModelController 
    {
        
        public bool Status { get; set; }
        public string Mesaj { get; set; }
        public string Url { get; set; }
        public string LinkText { get; set; }
    }
}