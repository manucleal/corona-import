using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.EntidadesNegocio
{
    [Table("vacunas")]
    public class Vacuna
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        public ICollection<Laboratorio> ListaLaboratorios { get; set; } = new List<Laboratorio>();
    }
}
