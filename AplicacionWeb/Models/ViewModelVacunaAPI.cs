using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionWeb.Models
{
    public class ViewModelVacunaAPI
    {

        public IEnumerable<VacunasAPI> Vacunas { get; set; }
        public string TipoFiltro { get; set; }
        public decimal PrecioMinFiltro { get; set; }
        public decimal PrecioMaxFiltro { get; set; }
        public int FaseClinicaDeAprobacionFiltro { get; set; }
        public string LaboratorioFiltro { get; set; }
        public string PaisFiltro { get; set; }

        public ViewModelVacunaAPI() { }
    }

    public class VacunasAPI
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public decimal Precio { get; set; }
        public int FaseClinicaDeAprobacion { get; set; }
        public string Labs { get; set; }
        public string Paises { get; set; }
    }
}