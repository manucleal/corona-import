namespace Dominio.DtosRepositorio
{
    public class VacunaFilterDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public decimal Precio { get; set; }        
        public int FaseClinicaDeAprobacion { get; set; }
        public string Labs { get; set; }
        public string Paises { get; set; }

        public VacunaFilterDTO() { }
    }
}
