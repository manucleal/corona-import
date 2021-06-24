using Dominio.EntidadesNegocio;
using System.Collections.Generic;

namespace Dominio.InterfacesRepositorio
{
    public interface IRepositorioLaboratorio
    {
        bool Add(Laboratorio laboratorio);

        Laboratorio FindById(int id);

        IEnumerable<Laboratorio> FindAll();
    }
}
