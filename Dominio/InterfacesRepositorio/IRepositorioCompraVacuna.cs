using Dominio.EntidadesNegocio;
using System;
using System.Collections.Generic;

namespace Dominio.InterfacesRepositorio
{
    public interface IRepositorioCompraVacuna
    {
        IEnumerable<CompraVacuna> FindAllByMutualista(int id);
    }
}
