using Dominio.EntidadesNegocio;
using System.ComponentModel.DataAnnotations;

namespace AplicacionWeb.Models
{
    public class ViewModelMutualista
    {
        public ViewModelMutualista() { }

        [Range(1, 999999, ErrorMessage = "El valor {0} no puede comenzar con 0")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El nombre de contacto es obligatorio")]
        public string NombreContacto { get; set; }

        [Required(ErrorMessage = "El monto tope de compras es obligatorio")]
        public int TopeComprasMens { get; set; }

        [Required(ErrorMessage = "La cantidad de afiliados es obligatoria")]
        public int CantAfiliados { get; set; }

        [Required(ErrorMessage = "El monto tope de vacunas por afiliado es obligatorio")]
        public int TopeGastoVacunaPorAfi { get; set; }

        public static Mutualista MapearAMutualista(ViewModelMutualista viewModelMutualista)
        {
            if (viewModelMutualista == null)
                return null;
            return new Mutualista
            {
                Id = viewModelMutualista.Id,
                Nombre = viewModelMutualista.Nombre,
                Telefono = viewModelMutualista.Telefono,
                NombreContacto = viewModelMutualista.NombreContacto,
                TopeComprasMensuales = viewModelMutualista.TopeComprasMens,
                CantidadAfiliados = viewModelMutualista.CantAfiliados,
                MontoMaxVacunasPorAfiliado = viewModelMutualista.TopeGastoVacunaPorAfi
            };
        }
    }
}