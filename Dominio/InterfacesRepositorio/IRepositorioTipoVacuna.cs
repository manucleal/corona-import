using Dominio.EntidadesNegocio;
using System.Collections.Generic;

namespace Dominio.InterfacesRepositorio
{
    public interface IRepositorioTipoVacuna
    {

        bool Add(TipoVacuna tipoVacuna);

        TipoVacuna FindById(string id);

        IEnumerable<TipoVacuna> FindAll();
    }
}
