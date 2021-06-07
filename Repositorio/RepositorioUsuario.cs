using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos.Contexto;
using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorio;

namespace Repositorio
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        public bool Add(Usuario unUsuario)
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    dataBase.Usuarios.Add(unUsuario);
                    dataBase.SaveChanges();
                }
                return true;
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Assert(false, "Error al ingresar Usuario" + exp.Message);
                return false;
            }
        }

        public Usuario FindById(string documento)
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    var resultado = dataBase.Usuarios
                        .Where(u => u.Documento == documento).FirstOrDefault();

                    return resultado;
                }                
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Assert(false, "Error al obtener Usuario" + exp.Message);
                return null;
            }
        }

        public Usuario Login(Usuario unUsuario)
        {
            throw new NotImplementedException();
        }
    }
}
