using Dominio.EntidadesNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.InterfacesRepositorio
{
    public interface IRepositorioMutualista
    {
        bool Add(Mutualista unaMutualista);

        Mutualista FindById(int id);

        Mutualista FindByName(string nombre);

        IEnumerable<Mutualista> FindAll();

        decimal CalcularMontoTotalCompras(int id);
    }
}
