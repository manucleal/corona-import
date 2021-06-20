using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Dominio.EntidadesNegocio
{
    [Table("CompraVacunas")]
    public class CompraVacuna
    {
        public int Id { get; set; }
        public Mutualista Mutualista { get; set; }
        public Vacuna Vacuna { get; set; }
        public int CantidadDosis { get; set; }
        public DateTime Fecha { get; set; }
        public int Monto { get; set; } 
        public int PrecioUnitario { get; set; }

        public CompraVacuna() { }
    }
}
