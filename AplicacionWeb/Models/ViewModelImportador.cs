using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dominio.EntidadesNegocio;

namespace AplicacionWeb.Models
{
    public class ViewModelImportador
    {
        public IEnumerable<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public IEnumerable<Vacuna> Vacunas { get; set; } = new List<Vacuna>();
        public IEnumerable<TipoVacuna> Tipos { get; set; } = new List<TipoVacuna>();
        public IEnumerable<Laboratorio> Laboratorios { get; set; } = new List<Laboratorio>();

        public ViewModelImportador() { }
    }
}