using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.EntidadesNegocio
{
    public class TipoVacuna
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }

        public TipoVacuna() { }
    }
}
