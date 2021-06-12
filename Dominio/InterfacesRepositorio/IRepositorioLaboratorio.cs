using Dominio.EntidadesNegocio;
using System.Collections.Generic;

namespace Dominio.InterfacesRepositorio
{
    public interface IRepositorioLaboratorio
    {
        IEnumerable<Laboratorio> FindAll();
    }
}
