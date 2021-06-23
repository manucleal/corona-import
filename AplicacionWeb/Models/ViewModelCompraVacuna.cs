using Dominio.EntidadesNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionWeb.Models
{
    public class ViewModelCompraVacuna
    {
        public ViewModelCompraVacuna() { }

        //indicando su fecha, la vacuna que compró, la cantidad de unidades de esa vacuna, 
        //su precio unitario y el monto de cada compra.
        public IEnumerable<CompraVacuna> CompraVacunasMutualista { get; set; }
    }
}