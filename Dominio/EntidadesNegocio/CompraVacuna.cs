using System;
using System.ComponentModel.DataAnnotations.Schema;

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
        public decimal Monto { get; set; } 

        public CompraVacuna() { }

        public static decimal ObtenerMontoCompra(int cantidadDosis, decimal precio)
        {
            if (cantidadDosis < 1 && precio <= 1) return -1;
            return cantidadDosis * precio;
        }
    }
}
