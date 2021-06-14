using Dominio.EntidadesNegocio;
using System.Collections.Generic;
using System.Linq;

namespace Dominio.InterfacesRepositorio
{
    public interface IRepositorioUsuario
    {
        bool Add(Usuario unUsuario);

        Usuario FindById(string documento);

        Usuario Login(Usuario unUsuario);

        Usuario ContadorLogin(Usuario usuario);

        bool CambiarPassword(string documento, string password);
    }
}
