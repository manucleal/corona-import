using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Dominio.EntidadesNegocio;
using Repositorio;

namespace Importador
{
    public class ManejadorArchivo
    {
        private static string raiz = AppDomain.CurrentDomain.BaseDirectory + "..\\Importador\\ArchivosImport";
        private static string[] dias = new string[7] {"Lun", "Mar", "Mier", "Jue", "Vie", "Sab", "Dom"};
        private static string delimitador = "|";
        RepositorioUsuario repositorioUsuario = new RepositorioUsuario();

        public List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> retorno = new List<Usuario>();
            using (StreamReader streamReader = File.OpenText(raiz + "\\Usuarios.txt"))
            {
                string linea = streamReader.ReadLine();
                while ((linea != null))
                {
                    if (linea.IndexOf(delimitador) > 0)
                    {
                        retorno.Add(ObtenerUsuarioDesdeString(linea, delimitador));
                        InsertarUsuario(ObtenerUsuarioDesdeString(linea, delimitador));
                    }
                    linea = streamReader.ReadLine();
                }
            }
            return retorno;
        }

        private static Usuario ObtenerUsuarioDesdeString(string linea, string delimitador)
        {
            string[] datos = linea.Split(delimitador.ToCharArray());
            if (datos.Length > 0) 
            {
                return new Usuario
                {
                    Documento = datos[0],
                    Password = GenerarPassword(datos[0])
                };
            }
            return null;
        }

        private bool InsertarUsuario(Usuario usuario)
        {            
            if (usuario != null && repositorioUsuario.FindById(usuario.Documento) == null)
            { 
                return repositorioUsuario.Add(usuario);
            }
            return false;
        }

        private static string GenerarPassword(string documento)
        {
            return documento.Substring(0,4) + "-" + dias[(int)new DateTime().DayOfWeek - 1];
        }
    }
}
