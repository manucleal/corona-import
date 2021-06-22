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

        public IEnumerable<Usuario> FindAll()
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    return dataBase.Usuarios.ToList();
                }
            }
            catch (Exception ex) { return null; }
        }

        public Usuario Login(Usuario unUsuario)
        {
            Usuario usuariobd = this.FindById(unUsuario.Documento);
            if (usuariobd != null && (usuariobd.Password == Usuario.EncodePasswordToBase64(unUsuario.Password) || usuariobd.Password == unUsuario.Password))
            {
                Usuario user = ContadorLogin(usuariobd);
                if (user.CantidadLogin > 1 && usuariobd.Password == unUsuario.Password)
                {
                    return new Usuario();
                }
                return user;
            }
            return new Usuario();
        }

        public Usuario ContadorLogin(Usuario usuario)
        {
            using (CoronaImportContext dataBase = new CoronaImportContext())
            {
                var user = dataBase.Usuarios.Where(u => u.Documento == usuario.Documento).FirstOrDefault();
                user.CantidadLogin += 1;
                dataBase.SaveChanges();
                return user;
            }
        }

        public bool CambiarPassword(string documento,string password)
        {
            using (CoronaImportContext db = new CoronaImportContext())
            {
                var user = db.Usuarios.Where(u => u.Documento == documento).FirstOrDefault();
                user.Password = Usuario.EncodePasswordToBase64(password);
                db.SaveChanges();
                return true;
            }
        }
    }
}