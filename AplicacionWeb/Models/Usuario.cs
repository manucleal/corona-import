using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionWeb.Models
{
    public class Usuariom
    {
        public Usuariom() { }

        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}