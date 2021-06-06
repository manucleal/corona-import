using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Dominio.EntidadesNegocio;
using AccesoDatos.Contexto;

namespace Importador
{
    public static class ManejadorArchivo
    {
        private static string raiz = AppDomain.CurrentDomain.BaseDirectory + "..\\Importador\\ArchivosImport";
        private static string[] dias = new string[7] {"Lun", "Mar", "Mier", "Jue", "Vie", "Sab", "Dom"};

        public static Usuario Leer(string documento)
        {
            StreamReader streamReader = null;

            using (streamReader = new StreamReader(raiz + "\\Usuarios.txt"))
            {
                string linea = streamReader.ReadLine();
                while (linea != null)
                {
                    Usuario unUsuario = ManejadorArchivo.ObtenerUsuarioDesdeString(linea, "|");
                    if (unUsuario != null)
                    {
                        if (unUsuario.Documento.Equals(documento, StringComparison.CurrentCultureIgnoreCase))
                        {
                            return unUsuario;
                        }
                    }
                    linea = streamReader.ReadLine();
                }
            }
            return null;
        }


        public static List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> retorno = new List<Usuario>();
            using (StreamReader streamReader = File.OpenText(raiz + "\\Usuarios.txt"))
            {
                string linea = streamReader.ReadLine();
                while ((linea != null))
                {
                    if (linea.IndexOf("|") > 0)
                    {
                        retorno.Add(ObtenerUsuarioDesdeString(linea, "|"));
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
            else
                return null;
        }

        private static bool InsertarUsuario(Usuario usuario)
        {
            //Usuario usuario = new Usuario() { Id = 1, Nombre = "Emanuel" };
            //using (CoronaImportContext db = new CoronaImportContext())
            //{
            //    db.Usuarios.Add(usuario);
            //    db.SaveChanges();
            //}
            return true;
        }

        private static string GenerarPassword(string documento)
        {
            return documento.Substring(0,4) + "-" + dias[(int)new DateTime().DayOfWeek - 1];
        }
    }
}
