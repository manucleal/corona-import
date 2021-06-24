using System;
using Dominio.EntidadesNegocio;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionWeb.Models
{
    public class CompraVacunaAPI
    {
        public Mutualista Mutualista { get; set; }
        public int IdVacuna { get; set; }
        public int CantidadDosis { get; set; }
        public decimal Monto { get; set; }

        public CompraVacunaAPI() { }
    }
}