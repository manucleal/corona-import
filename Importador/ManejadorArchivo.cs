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
        private static string delimitador = "|";
        private RepositorioUsuario repositorioUsuario;
        private RepositorioLaboratorio repositorioLaboratorio;
        private RepositorioTipoVacuna repositorioTipoVacuna;

        private ManejadorArchivo() {
            this.repositorioUsuario = new RepositorioUsuario();
            this.repositorioLaboratorio = new RepositorioLaboratorio();
            this.repositorioTipoVacuna = new RepositorioTipoVacuna();
        }

        public static void ImportarDatos()
        {
            ManejadorArchivo manejador = new ManejadorArchivo();
            ObtenerDatos(manejador);
        }

        private static void ObtenerDatos(ManejadorArchivo manejador)
        {
            using (StreamReader streamReader = File.OpenText(raiz + "\\Usuarios.txt"))
            {
                string linea = streamReader.ReadLine();
                while ((linea != null))
                {
                    if (linea.IndexOf(delimitador) > 0)
                    {
                        InsertarUsuario(ObtenerUsuarioDesdeString(linea, delimitador), manejador.repositorioUsuario);
                    }
                    linea = streamReader.ReadLine();
                }
            }
            using (StreamReader streamReader = File.OpenText(raiz + "\\TipoVacunas.txt"))
            {
                string linea = streamReader.ReadLine();
                while ((linea != null))
                {
                    if (linea.IndexOf(delimitador) > 0)
                    {
                        InsertarTipoVacuna(ObtenerTipoVacunasDesdeString(linea, delimitador), manejador.repositorioTipoVacuna);
                    }
                    linea = streamReader.ReadLine();
                }
            }
            using (StreamReader streamReader = File.OpenText(raiz + "\\Laboratorios.txt"))
            {
                string linea = streamReader.ReadLine();
                while ((linea != null))
                {
                    if (linea.IndexOf(delimitador) > 0)
                    {
                        InsertarLaboratorio(ObtenerLaboratoriosDesdeString(linea, delimitador), manejador.repositorioLaboratorio);
                    }
                    linea = streamReader.ReadLine();
                }
            }
        }

        private static Usuario ObtenerUsuarioDesdeString(string linea, string delimitador)
        {
            string[] datos = linea.Split(delimitador.ToCharArray());
            if (datos.Length > 0) 
            {
                return new Usuario
                {
                    Documento = datos[0],
                    Password = Usuario.GenerarPassword(datos[0])
                };
            }
            return null;
        }

        private static TipoVacuna ObtenerTipoVacunasDesdeString(string linea, string delimitador)
        {
            string[] datos = linea.Split(delimitador.ToCharArray());
            if (datos.Length > 0)
            {
                return new TipoVacuna
                {
                    Id = datos[0],
                    Descripcion = datos[1]
                };
            }
            return null;
        }

        private static Laboratorio ObtenerLaboratoriosDesdeString(string linea, string delimitador)
        {
            string[] datos = linea.Split(delimitador.ToCharArray());
            if (datos.Length > 0)
            {
                return new Laboratorio
                {
                    Id = int.Parse(datos[0].Trim()),
                    Nombre = datos[1],
                    PaisOrigen = datos[2],
                    Experiencia = (datos[3].Trim() == "Si") ? true : false
                };
            }
            return null;
        }

        private static bool InsertarUsuario(Usuario usuario, RepositorioUsuario repositorioUsuario)
        {            
            if (usuario != null && repositorioUsuario.FindById(usuario.Documento) == null)
            { 
                return repositorioUsuario.Add(usuario);
            }
            return false;
        }

        private static bool InsertarLaboratorio(Laboratorio laboratorio, RepositorioLaboratorio repositorioLaboratorio)
        {
            if (laboratorio != null && repositorioLaboratorio.FindById(laboratorio.Id) == null)
            {
                return repositorioLaboratorio.Add(laboratorio);
            }
            return false;
        }

        private static bool InsertarTipoVacuna(TipoVacuna tipoVacuna, RepositorioTipoVacuna repositorioTipoVacuna)
        {
            if (tipoVacuna != null && repositorioTipoVacuna.FindById(tipoVacuna.Id) == null)
            {
                return repositorioTipoVacuna.Add(tipoVacuna);
            }
            return false;
        }
    }
}
