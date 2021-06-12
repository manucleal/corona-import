using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dominio.EntidadesNegocio;

namespace AplicacionWeb.Models
{
    public class ViewModelUsuario
    {
        public ViewModelUsuario() { }

        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public static Usuario MapearAUsuario(ViewModelUsuario viewModelUsuario)
        {
            if (viewModelUsuario == null)
                return null;
            return new Usuario
            {
                Documento = viewModelUsuario.Documento,
                Nombre = viewModelUsuario.Nombre,
                Password = viewModelUsuario.Password
            };
        }
    }
}