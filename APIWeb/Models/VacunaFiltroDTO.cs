using Dominio.EntidadesNegocio;

namespace APIWeb.Models
{
    public class VacunaFiltroDTO
    {
        public int FaseClinicaAprob { get; set; }
        public decimal PrecioMin { get; set; }
        public decimal PrecioMax { get; set; }
        public string Tipo { get; set; }
        public string Laboratorio { get; set; }
        public string PaisAceptada { get; set; }

        public VacunaFiltroDTO() { }
    }
}