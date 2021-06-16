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
        private static readonly string raiz = AppDomain.CurrentDomain.BaseDirectory + "..\\Importador\\ArchivosImport";
        private static readonly string delimitador = "|";
        private RepositorioUsuario repositorioUsuario;
        private RepositorioLaboratorio repositorioLaboratorio;
        private RepositorioTipoVacuna repositorioTipoVacuna;
        private RepositorioVacuna repositorioVacuna;

        private ManejadorArchivo() {
            this.repositorioUsuario = new RepositorioUsuario();
            this.repositorioLaboratorio = new RepositorioLaboratorio();
            this.repositorioTipoVacuna = new RepositorioTipoVacuna();
            this.repositorioVacuna = new RepositorioVacuna();
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
            using (StreamReader streamReader = File.OpenText(raiz + "\\Vacunas.txt"))
            {
                string linea = streamReader.ReadLine();
                while ((linea != null))
                {
                    if (linea.IndexOf(delimitador) > 0)
                    {
                        InsertarVacuna(ObtenerVacunaDesdeString(linea, delimitador), manejador.repositorioVacuna);
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
                    Nombre = datos[1],
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

        private static Vacuna ObtenerVacunaDesdeString(string linea, string delimitador)
        {
            string[] datos = linea.Split(delimitador.ToCharArray());
            if (datos.Length > 0)
            {
                //TODO: obtener array de paises del archivo statusVacuna segun el id de la vacuna 
                //TODO: obtener nombre de tipo de vacuna para crear el objeto TipoVacuna completo 
                //"{id} | {idTipoVac} | {idUsuario} | {nombre} | {cantDosis} | {lapsoDiasDosis} | {maxEdad} | " +
                //$"{minEdad} | {eficaciaPrev} | {eficaciaHosp} | {eficaciaCti} | {maxTemp} | {minTemp} | {produccionAnual}" +
                //$" | {faseClinicaAprob} | {emergencia} | {efectosAdversos} | {precio} | {ultimaModificacion} | {covax}"
                return new Vacuna
                {
                    Id = int.Parse(datos[0].Trim()),
                    TipoVacuna = new TipoVacuna { Id = datos[1], Descripcion = "obtener este dato desde TipoVacuna.txt" },
                    Documento = datos[2].Trim(),
                    Nombre = datos[3].Trim(),
                    CantidadDosis = int.Parse(datos[4].Trim()),
                    LapsoDiasDosis = int.Parse(datos[5].Trim()),
                    MaxEdad = int.Parse(datos[6].Trim()),
                    MinEdad = int.Parse(datos[7].Trim()),                    
                    EficaciaPrev = int.Parse(datos[8].Trim()),
                    EficaciaHosp = int.Parse(datos[9].Trim()),
                    EficaciaCti = int.Parse(datos[10].Trim()),
                    MaxTemp = int.Parse(datos[11].Trim()),
                    MinTemp = int.Parse(datos[12].Trim()),                    
                    ProduccionAnual = long.Parse(datos[13].Trim()),
                    FaseClinicaAprob = int.Parse(datos[14].Trim()),                   
                    Emergencia = (datos[15].Trim().ToUpper() == "SI") ? true : false,
                    EfectosAdversos = datos[16],
                    Precio = decimal.Parse(datos[17].Trim()),
                    UltimaModificacion = Usuario.GenerarDateTime(datos[18]),
                    Covax = (datos[19].Trim().ToUpper() == "SI") ? true : false
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

        private static bool InsertarVacuna(Vacuna vacuna, RepositorioVacuna repositorioVacuna)
        {
            if (vacuna != null && repositorioVacuna.FindById(vacuna.Id) == null)
            {               
                bool aux = repositorioVacuna.Add(vacuna);
                return aux;
            }
            return false;
        }
    }
}
