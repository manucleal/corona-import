using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionWeb.Models
{
    public class ViewModelVacunaAPI
    {

        public IEnumerable<VacunasAPI> Vacunas { get; set; }
        public ModelFiltro Filtro { get; set; }

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

    public class ModelFiltro
    {
        public string FaseClinicaDeAprobacionFiltro { get; set; }
        public string TipoFiltro { get; set; }
        public string PrecioMinFiltro { get; set; }
        public string PrecioMaxFiltro { get; set; }
        public string LaboratorioFiltro { get; set; }
        public string PaisFiltro { get; set; }
        public string Criterio { get; set; }
    }
}