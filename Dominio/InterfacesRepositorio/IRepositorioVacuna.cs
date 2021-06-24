using Dominio.EntidadesNegocio;
using Dominio.DtosRepositorio;
using System.Collections.Generic;

namespace Dominio.InterfacesRepositorio
{
    public interface IRepositorioVacuna
    {
        bool Add(Vacuna unaVacuna);

        Vacuna FindById(int idVacuna);

        IEnumerable<Vacuna> FindAll();

        IEnumerable<VacunaFilterDTO> FindAllByFiltersOR(int faseClinicaAprob, int PrecioMin, int PrecioMax, string tipo, string laboratorio, string paisAceptada);

        IEnumerable<VacunaFilterDTO> FindAllByFiltersAND(int faseClinicaAprob, int PrecioMin, int PrecioMax, string tipo, string laboratorio, string paisAceptada);

        bool AddCompra(CompraVacuna compra);
    }
}
