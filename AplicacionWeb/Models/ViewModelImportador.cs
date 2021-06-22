using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dominio.EntidadesNegocio;

namespace AplicacionWeb.Models
{
    public class ViewModelImportador
    {
        public IEnumerable<Usuario> Usuarios { get; set; }
        public IEnumerable<Vacuna> Vacunas { get; set; }

        public ViewModelImportador() { }
    }
}