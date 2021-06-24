using Dominio.EntidadesNegocio;
using System.ComponentModel.DataAnnotations;

namespace AplicacionWeb.Models
{
    public class ViewModelMutualista
    {
        public ViewModelMutualista() { }

        [Range(1, 999999, ErrorMessage = "El valor no es válido")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(50, ErrorMessage = "No debe superar los 50 caracteres")]
        [MinLength(3, ErrorMessage = "Debe tener al menos 3 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [MaxLength(10, ErrorMessage = "No debe superar los 10 caracteres")]
        [MinLength(3, ErrorMessage = "Debe tener al menos 3 caracteres")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El nombre de contacto es obligatorio")]
        [MaxLength(50, ErrorMessage = "No debe superar los 50 caracteres")]
        [MinLength(3, ErrorMessage = "Debe tener al menos 3 caracteres")]
        public string NombreContacto { get; set; }

        [Required(ErrorMessage = "El monto tope de compras es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor no es válido, debe ser positivo")]
        public int TopeComprasMens { get; set; }

        [Required(ErrorMessage = "La cantidad de afiliados es obligatoria")]
        [Range(0, 1000000, ErrorMessage = "El valor no es válido, debe estar entre 0 y 1.000.000")]
        public int CantAfiliados { get; set; }

        [Required(ErrorMessage = "El monto tope máximo por afiliado es obligatorio")]
        [Range(0, 1000, ErrorMessage = "El valor no es válido, debe estar entre 0 y 1.000")]
        public int TopeGastoVacunaPorAfi { get; set; }

        public static Mutualista MapearAMutualista(ViewModelMutualista viewModelMutualista)
        {
            if (viewModelMutualista == null)
                return null;
            return new Mutualista
            {
                Codigo = viewModelMutualista.Codigo,
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