using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.EntidadesNegocio
{
    
    [Table("Mutualistas")]
    public class Mutualista
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Range(1, 999999)]
        public int Codigo { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public string NombreContacto { get; set; }

        [Required]
        public int TopeComprasMensuales { get; set; }

        [Required]
        public int CantidadAfiliados { get; set; }

        [Required]
        public int MontoMaxVacunasPorAfiliado { get; set; }

        [Required]
        public int ComprasRealizadas { get; set; }

        public Mutualista() { }
        
        public static decimal ObtenerMontoAutorizado(Mutualista mutualista)
        {
           if (mutualista == null) return -1;
           return mutualista.MontoMaxVacunasPorAfiliado * mutualista.CantidadAfiliados;
        }
    }
}
