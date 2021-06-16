using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.EntidadesNegocio
{
    [Table("Laboratorios")]
    public class Laboratorio
    {
        [Key]
        [Column(Order = 1)]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string PaisOrigen { get; set; }
        public bool Experiencia { get; set; }

        public virtual ICollection<Vacuna> Vacunas { get; set; } = new List<Vacuna>();

        public Laboratorio() { }
    }
}
