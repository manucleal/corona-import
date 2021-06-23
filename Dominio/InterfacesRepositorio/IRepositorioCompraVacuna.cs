using Dominio.EntidadesNegocio;
using System;
using System.Collections.Generic;

namespace Dominio.InterfacesRepositorio
{
    interface IRepositorioCompraVacuna
    {
        IEnumerable<CompraVacuna> FindAllByMutualista();
    }
}
