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
    public class RepositorioLaboratorio : IRepositorioLaboratorio
    {

        public bool Add(Laboratorio laboratorio)
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    dataBase.Laboratorios.Add(laboratorio);
                    dataBase.SaveChanges();
                }
                return true;
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Assert(false, "Error al ingresar Laboratorio" + exp.Message);
                return false;
            }
        }

        public Laboratorio FindById(int id)
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    var resultado = dataBase.Laboratorios
                        .Where(l => l.Id == id).FirstOrDefault();

                    return resultado;
                }
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Assert(false, "Error al obtener Laboratorio" + exp.Message);
                return null;
            }
        }

        public IEnumerable<Laboratorio> FindAll()
        {
            try
            {
                //Conexion manejadorConexion = new Conexion();
                //SqlConnection con = manejadorConexion.CrearConexion();
                //SqlCommand cmd = new SqlCommand("SELECT * FROM Laboratorios", con);

                //manejadorConexion.AbrirConexion(con);
                //SqlDataReader dataReader = cmd.ExecuteReader();
                //manejadorConexion.CerrarConexion(con);
                List<Laboratorio> laboratorios = new List<Laboratorio>();

                //while (dataReader.Read())
                //{
                //    laboratorios.Add(new Laboratorio
                //    {
                //        Id = (int)dataReader["Id"],
                //        Nombre = (string)dataReader["Nombre"],
                //        PaisOrigen = (string)dataReader["PaisOrigen"],
                //        Experiencia = (bool)dataReader["Experiencia"],
                //    });
                //}
                return laboratorios;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Assert(false, "Error al ingresar Laboratorio" + e.Message);
                return null;
            }
        }
    }
}
