using Dominio.EntidadesNegocio;
using System.Collections.Generic;

namespace Dominio.InterfacesRepositorio
{
    public interface IRepositorioVacuna
    {
        bool Add(Vacuna unaVacuna);

        Vacuna FindById(int idVacuna);

        IEnumerable<Vacuna> FindAll();

        IEnumerable<Vacuna> FindAllByName(string nombre);

        IEnumerable<Vacuna> FindAllByApprovalPhase(int FaseClinicaAprob);

        IEnumerable<Vacuna> FindAllByCountry(string pais);

        IEnumerable<Vacuna> FindAllByMaxPrice(decimal precioMax);

        IEnumerable<Vacuna> FindAllByMinPrice(decimal precioMin);

        IEnumerable<Vacuna> FindAllByTypeVac(string idTipo);

        IEnumerable<Vacuna> FindAllByLabName(string nombreLab);
    }
}
