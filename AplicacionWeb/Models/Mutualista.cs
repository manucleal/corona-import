using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dominio.EntidadesNegocio;

namespace AplicacionWeb.Models
{
    public class ViewModelMutualista
    {
        public ViewModelMutualista() { }

        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string NombreContacto { get; set; }
        public string TopeComprasMens { get; set; }
        public string CantAfiliados { get; set; }
        public string TopeGastoVacunaPorAfi { get; set; }

        //public static Mutualista MapearAMutualista(ViewModelMutualista viewModelMutualista)
        //{
        //    if (viewModelMutualista == null)
        //        return null;
        //    return new Mutualista
        //    {
        //         = ViewModelMutualista.,
        //         = ViewModelMutualista.,
        //         = ViewModelMutualista.,
        //         = ViewModelMutualista.,
        //         = ViewModelMutualista.,
        //         = ViewModelMutualista.,
        //         = ViewModelMutualista.
        //    };
        //}
    }
}