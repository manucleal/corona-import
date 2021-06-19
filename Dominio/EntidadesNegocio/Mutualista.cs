using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.EntidadesNegocio
{
    [Table("Mutualistas")]
    public class Mutualista
    {
        [Key]
        [Range(1, 999999)]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } //TODO: agregar unique

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
        
        public Mutualista() { }
    }
}
