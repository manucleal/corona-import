using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionWeb.Models
{
    public class ViewModelVacunaAPI
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public decimal Precio { get; set; }
        public int FaseClinicaDeAprobacion { get; set; }
        public string Labs { get; set; }
        public string Paises { get; set; }

        public ViewModelVacunaAPI() { }
    }
}