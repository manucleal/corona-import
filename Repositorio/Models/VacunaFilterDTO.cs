using System;
using System.Collections.Generic;
using Dominio.EntidadesNegocio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Models
{
    public class VacunaFilterDTO
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public decimal Precio { get; set; }        
        public int FaseClinicaDeAprobacion { get; set; }
        public ICollection<Laboratorio> Laboratorios { get; set; }
        public string Paises { get; set; }

        public VacunaFilterDTO() { }
    }
}
