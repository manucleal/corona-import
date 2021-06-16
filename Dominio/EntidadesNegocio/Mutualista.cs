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
        [Range(1, 999999, ErrorMessage = "El valor {0} debe estar entre {1} y {2}.")]
        public int Id { get; set; }
        public string Nombre { get; set; } //TODO: agregar unique
        public string Telefono { get; set; }
        public string NombreContacto { get; set; }
        public int TopeComprasMensuales { get; set; }
        public int CantidadAfiliados { get; set; }
        public string MontoMaxVacunasPorAfiliado { get; set; }        
        
        public Mutualista() { }
    }
}
