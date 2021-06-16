using Dominio.EntidadesNegocio;

namespace APIWeb.Models
{
    public class VacunaModel
    {
        public string Nombre { get; set; }
        public TipoVacuna TipoVacuna { get; set; }
        public decimal Precio { get; set; }

        public VacunaModel() { }
    }
}