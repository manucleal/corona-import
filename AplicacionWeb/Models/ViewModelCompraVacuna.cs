using Dominio.EntidadesNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionWeb.Models
{
    public class ViewModelCompraVacuna
    {
        public decimal MontoAutorizado { get; set; }
        public decimal SaldoDisponible { get; set; }
        public int ComprasDisponibles { get; set; }

        public ViewModelCompraVacuna() { }
        
        public IEnumerable<CompraVacuna> CompraVacunasMutualista { get; set; }

    }
}