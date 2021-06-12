using Dominio.EntidadesNegocio;
using System.Collections.Generic;

namespace Dominio.InterfacesRepositorio
{
    public interface IRepositorioTipoVacuna
    {
        IEnumerable<TipoVacuna> FindAll();
    }
}
