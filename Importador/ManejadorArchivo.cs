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
            using (StreamReader streamReader = File.OpenText(Path.Combine(raiz, "Usuarios.txt")))
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
            using (StreamReader streamReader = File.OpenText(Path.Combine(raiz, "TipoVacunas.txt")))
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
            using (StreamReader streamReader = File.OpenText(Path.Combine(raiz, "Laboratorios.txt")))
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
            using (StreamReader streamReader = File.OpenText(Path.Combine(raiz, "Vacunas.txt")))
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
                    Id = datos[0].Trim(),
                    Descripcion = datos[1].Trim()
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
                    Nombre = datos[1].Trim(),
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
                string IdLaboratorio = datos[0].Trim();
                string IdVacuna = datos[0].Trim();
                return new Vacuna
                {
                    Id = int.Parse(IdVacuna),
                    Tipo = BuscarYObtenerTipoVacuna(datos[1].Trim()),
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
                    Covax = (datos[19].Trim().ToUpper() == "SI") ? true : false,
                    Laboratorios = BuscarYObtenerLaboratoriosVacuna(IdLaboratorio),
                    Paises = BuscarYObtenerPaisesVacuna(IdVacuna)
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

        private static TipoVacuna BuscarYObtenerTipoVacuna(string clave)
        {
            StreamReader streamReader = null;
            using (streamReader = new StreamReader(Path.Combine(raiz, "TipoVacunas.txt")))
            {
                string linea = streamReader.ReadLine();
                while (linea != null)
                {
                    TipoVacuna tipoVacuna = ObtenerTipoVacunasDesdeString(linea, delimitador);
                    if (tipoVacuna != null)
                    {
                        if (tipoVacuna.Id.Equals(clave, StringComparison.CurrentCultureIgnoreCase))
                        {
                            return tipoVacuna;
                        }
                    }
                    linea = streamReader.ReadLine();
                }
            }
            return null;
        }

        private static ICollection<Laboratorio> BuscarYObtenerLaboratoriosVacuna(string claveVacuna)
        {
            ICollection<Laboratorio> laboratorios = new List<Laboratorio>();
            using (StreamReader streamReader = File.OpenText(Path.Combine(raiz, "VacunaLaboratorios.txt")))
            {
                string linea = streamReader.ReadLine();
                while (linea != null)
                {
                    string[] vacunaLaboratorios = linea.Split(delimitador.ToCharArray());
                    string IdVacuna = vacunaLaboratorios[0].Trim();

                    if (IdVacuna.Equals(claveVacuna, StringComparison.CurrentCultureIgnoreCase))
                    {
                        string IdLaboratorio = vacunaLaboratorios[1].Trim();
                        using (StreamReader streamReaderLab = File.OpenText(Path.Combine(raiz, "Laboratorios.txt")))
                        {
                            string lineaLab = streamReaderLab.ReadLine();
                            while (lineaLab != null)
                            {
                                if (lineaLab.IndexOf(delimitador) > 0)
                                {
                                    Laboratorio laboratorio = ObtenerLaboratoriosDesdeString(lineaLab, delimitador);
                                    if(laboratorio.Id == int.Parse(IdLaboratorio))
                                    {
                                        laboratorios.Add(laboratorio);
                                    }                                    
                                }
                                lineaLab = streamReaderLab.ReadLine();
                            }
                        }                        
                    }
                    linea = streamReader.ReadLine();
                }
            }
            return laboratorios;
        }

        private static string BuscarYObtenerPaisesVacuna(string claveVacuna)
        {
            ICollection<string> paises = new List<string>();
            using (StreamReader streamReader = File.OpenText(Path.Combine(raiz, "StatusVacuna.txt")))
            {
                string linea = streamReader.ReadLine();
                while (linea != null)
                {
                    string[] paisesVacuna = linea.Split(delimitador.ToCharArray());
                    string IdVacuna = paisesVacuna[0].Trim();

                    if (IdVacuna.Equals(claveVacuna, StringComparison.CurrentCultureIgnoreCase))
                    {
                        string IdPais = paisesVacuna[1].Trim();
                        using (StreamReader streamReaderPais = File.OpenText(Path.Combine(raiz, "Paises.txt")))
                        {
                            string lineaPais = streamReaderPais.ReadLine();
                            while (lineaPais != null)
                            {
                                if (lineaPais.IndexOf(delimitador) > 0)
                                {
                                    string[] datos = lineaPais.Split(delimitador.ToCharArray());
                                    string codigoPais = datos[0].Trim();
                                    string nombrePais = datos[1].Trim();
                                    if (codigoPais.Equals(IdPais, StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        paises.Add(nombrePais);
                                    }
                                }
                                lineaPais = streamReaderPais.ReadLine();
                            }
                        }
                    }
                    linea = streamReader.ReadLine();
                }
            }
            return String.Join(",", paises.ToArray());
        }
    }
}
