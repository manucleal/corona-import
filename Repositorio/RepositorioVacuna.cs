using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dominio.EntidadesNegocio;
using AccesoDatos.Contexto;
using Dominio.InterfacesRepositorio;

namespace Repositorio
{
    public class RepositorioVacuna
    {
        public bool Add(Vacuna unaVacuna)
        {            
            try
            {
                if (!unaVacuna.ValidateTemperature(unaVacuna) || !unaVacuna.ValidateAge(unaVacuna) || !unaVacuna.ValidateCantidadDosis(unaVacuna) || !unaVacuna.ValidateProduccionAnual(unaVacuna)) return false;
                unaVacuna.Precio = Vacuna.ValidatePrice(unaVacuna);
                unaVacuna.LapsoDiasDosis = Vacuna.ValidateLapsoDiasDosis(unaVacuna);
                using (var context = new CoronaImportContext())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {
                        context.Vacunas.Add(unaVacuna);
                        context.SaveChanges();

                        dbContextTransaction.Commit();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Assert(false, "Error al ingresar Vacuna" + e.Message);
                return false;
            }
        }

        public Vacuna FindById(int idVacuna)
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    var resultado = dataBase.Vacunas
                        .Where(v => v.Id == idVacuna).FirstOrDefault();

                    return resultado;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Assert(false, "Error al buscar vacuna" + e.Message);
                return null;
            }
        }

        public IEnumerable<Vacuna> FindAll()
        {
            //Conexion manejadorConexion = new Conexion();
            //SqlConnection con = manejadorConexion.CrearConexion();

            try
            {
                //SqlCommand cmd = new SqlCommand("SELECT * FROM Vacunas", con);
                //manejadorConexion.AbrirConexion(con);
                //SqlDataReader dataReader = cmd.ExecuteReader();
                
                List<Vacuna> vacunas = new List<Vacuna>();

                //while (dataReader.Read())
                //{
                //    int idVacuna = (int)dataReader["Id"];
                //    string idTipo = (string)dataReader["IdTipo"];
                //    Vacuna unaVacuna = new Vacuna()
                //    {
                //        Id = idVacuna,
                //        Nombre = (string)dataReader["Nombre"],
                //        IdTipo = idTipo,
                //        CantidadDosis = (int)dataReader["CantidadDosis"],
                //        LapsoDiasDosis = (int)dataReader["LapsoDiasDosis"],
                //        MaxEdad = (int)dataReader["MaxEdad"],
                //        MinEdad = (int)dataReader["MinEdad"],
                //        EficaciaPrev = (int)dataReader["EficaciaPrev"],
                //        EficaciaHosp = (int)dataReader["EficaciaHosp"],
                //        EficaciaCti = (int)dataReader["EficaciaCti"],
                //        MaxTemp = (int)dataReader["MaxTemp"],
                //        MinTemp = (int)dataReader["MinTemp"],
                //        ProduccionAnual = (long)dataReader["ProduccionAnual"],
                //        FaseClinicaAprob = (int)dataReader["FaseClinicaAprob"],
                //        Emergencia = (bool)dataReader["Emergencia"],
                //        EfectosAdversos = (string)dataReader["EfectosAdversos"],
                //        Precio = (decimal)dataReader["Precio"],
                //        IdUsuario = (string)dataReader["IdUsuario"]
                //    };

                //    vacunas.Add(unaVacuna);
                //    unaVacuna.ListaLaboratorios = AddLabsToVacunas(idVacuna, con);
                //    unaVacuna.TipoVacuna = AddTipoVacunaToVacunas(idTipo, con);
                //    unaVacuna.ListaPaises = AddPaisToVacunas(idVacuna, con);
                //}
                //dataReader.Close();

                return vacunas;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Assert(false, "Error al listar Vacuna" + e.Message);
                return null;
            }
            finally
            {
                //manejadorConexion.CerrarConexion(con);
            }
        }

        private ICollection<Laboratorio> AddLabsToVacunas(int idVacuna, SqlConnection con)
        {
            SqlCommand query = new SqlCommand("SELECT * FROM Laboratorios l " +
                                  "WHERE l.Id IN (SELECT IdLaboratorio FROM VacunaLaboratorios vl " +
                                  "WHERE IdVacuna=@IdVacuna)", con);
            query.Parameters.AddWithValue("@IdVacuna", idVacuna);
            SqlDataReader labs = query.ExecuteReader();
            ICollection<Laboratorio> ListaLaboratorios = new List<Laboratorio>();
            while (labs.Read())
            {
                Laboratorio lab = new Laboratorio()
                {
                    Id = (int)labs["Id"],
                    Nombre = (string)labs["Nombre"],
                    PaisOrigen = (string)labs["PaisOrigen"],
                    Experiencia = (bool)labs["Experiencia"]
                };

                ListaLaboratorios.Add(lab);
            }

            labs.Close();

            return ListaLaboratorios;
        }

        private TipoVacuna AddTipoVacunaToVacunas(string idTipo, SqlConnection con)
        {
            SqlCommand query = new SqlCommand("SELECT * FROM TipoVacunas " +                                  
                                  "WHERE Id LIKE @idTipo", con);
            query.Parameters.AddWithValue("@idTipo", '%' + idTipo + '%');

            SqlDataReader reader = query.ExecuteReader();
            TipoVacuna tipoVacuna = null;
            while (reader.Read())
            {
                TipoVacuna unaVacuna = new TipoVacuna()
                {
                    Id = (string)reader["Id"],
                    Descripcion = (string)reader["Descripcion"]
                };

                tipoVacuna = unaVacuna;
            }

            reader.Close();

            return tipoVacuna;
        }

        //private ICollection<Pais> AddPaisToVacunas(int idVacuna, SqlConnection con)
        //{
        //    SqlCommand query = new SqlCommand("SELECT * FROM Paises p WHERE p.CodPais IN(" +
        //                                      "SELECT s.CodPais FROM StatusVacuna s " +
        //                                      "WHERE s.IdVac=@idVacuna)", con);
        //    query.Parameters.AddWithValue("@idVacuna", idVacuna);

        //    SqlDataReader reader = query.ExecuteReader();
        //    ICollection<Pais> ListaPaises = new List<Pais>();

        //    while (reader.Read())
        //    {
        //        Pais unPais = new Pais()
        //        {
        //            CodPais = (string)reader["CodPais"],
        //            Nombre = (string)reader["Nombre"]
        //        };

        //        ListaPaises.Add(unPais);
        //    }

        //    reader.Close();

        //    return ListaPaises;
        //}

        //public IEnumerable<Vacuna> FindAllByName(string nombre)
        //{
        //    Conexion manejadorConexion = new Conexion();
        //    SqlConnection con = manejadorConexion.CrearConexion();

        //    try
        //    {
        //        SqlCommand query = new SqlCommand("Select * from Vacunas where Nombre = @Nombre", con);
        //        manejadorConexion.AbrirConexion(con);

        //        query.Parameters.AddWithValue("@Nombre", nombre);
        //        SqlDataReader dataReader = query.ExecuteReader();

        //        List<Vacuna> vacunas = new List<Vacuna>();

        //        while (dataReader.Read())
        //        {
        //            int idVacuna = (int)dataReader["Id"];
        //            string idTipo = (string)dataReader["IdTipo"];
        //            Vacuna unaVacuna = new Vacuna()
        //            {
        //                Id = idVacuna,
        //                Nombre = (string)dataReader["Nombre"],
        //                IdTipo = idTipo,
        //                Precio = (decimal)dataReader["Precio"],

        //            };
        //            vacunas.Add(unaVacuna);
        //            unaVacuna.ListaLaboratorios = AddLabsToVacunas(idVacuna, con);
        //            unaVacuna.TipoVacuna = AddTipoVacunaToVacunas(idTipo, con);
        //            unaVacuna.ListaPaises = AddPaisToVacunas(idVacuna, con);
        //        }
        //        return vacunas;
        //    }
        //    catch (Exception e)
        //    {
        //        System.Diagnostics.Debug.Assert(false, "Error al listar vacunas por nombre" + e.Message);
        //        return null;
        //    }
        //    finally
        //    {
        //        manejadorConexion.CerrarConexion(con);
        //    }
        //}

        //public IEnumerable<Vacuna> FindAllByApprovalPhase(int FaseClinicaAprob)
        //{
        //    Conexion manejadorConexion = new Conexion();
        //    SqlConnection con = manejadorConexion.CrearConexion();

        //    try
        //    {
        //        SqlCommand query = new SqlCommand("Select * from Vacunas where FaseClinicaAprob = @FaseClinicaAprob", con);
        //        manejadorConexion.AbrirConexion(con);

        //        query.Parameters.AddWithValue("@FaseClinicaAprob", FaseClinicaAprob);
        //        SqlDataReader dataReader = query.ExecuteReader();

        //        List<Vacuna> vacunas = new List<Vacuna>();

        //        while (dataReader.Read())
        //        {
        //            int idVacuna = (int)dataReader["Id"];
        //            string idTipo = (string)dataReader["IdTipo"];
        //            Vacuna unaVacuna = new Vacuna()
        //            {
        //                Id = idVacuna,
        //                Nombre = (string)dataReader["Nombre"],
        //                IdTipo = idTipo,
        //                Precio = (decimal)dataReader["Precio"],

        //            };
        //            vacunas.Add(unaVacuna);
        //            unaVacuna.ListaLaboratorios = AddLabsToVacunas(idVacuna, con);
        //            unaVacuna.TipoVacuna = AddTipoVacunaToVacunas(idTipo, con);
        //            unaVacuna.ListaPaises = AddPaisToVacunas(idVacuna, con);
        //        }
        //        return vacunas;
        //    }
        //    catch (Exception e)
        //    {
        //        System.Diagnostics.Debug.Assert(false, "Error al listar vacunas por fase clinica aprobacion" + e.Message);
        //        return null;
        //    }
        //    finally
        //    {
        //        manejadorConexion.CerrarConexion(con);
        //    }
        //}

        //public IEnumerable<Vacuna> FindAllByCountry(string pais)
        //{
        //    Conexion manejadorConexion = new Conexion();
        //    SqlConnection con = manejadorConexion.CrearConexion();

        //    try
        //    {
        //        SqlCommand query = new SqlCommand("SELECT * FROM Vacunas " +
        //                                           "WHERE Id IN (SELECT IdVacuna " +
        //                                                        "FROM VacunaLaboratorios " +
        //                                                        "WHERE IdLaboratorio IN (SELECT l.Id " +
        //                                                                                "FROM Laboratorios l, Paises p " +
        //                                                                                "WHERE l.PaisOrigen = p.CodPais AND (p.Nombre = @Pais OR p.CodPais = @Pais)))", con);
        //        manejadorConexion.AbrirConexion(con);

        //        query.Parameters.AddWithValue("@Pais", pais);
        //        SqlDataReader dataReader = query.ExecuteReader();

        //        List<Vacuna> vacunas = new List<Vacuna>();

        //        while (dataReader.Read())
        //        {
        //            int idVacuna = (int)dataReader["Id"];
        //            string idTipo = (string)dataReader["IdTipo"];
        //            Vacuna unaVacuna = new Vacuna()
        //            {
        //                Id = idVacuna,
        //                Nombre = (string)dataReader["Nombre"],
        //                IdTipo = idTipo,
        //                Precio = (decimal)dataReader["Precio"],

        //            };
        //            vacunas.Add(unaVacuna);
        //            unaVacuna.ListaLaboratorios = AddLabsToVacunas(idVacuna, con);
        //            unaVacuna.TipoVacuna = AddTipoVacunaToVacunas(idTipo, con);
        //            unaVacuna.ListaPaises = AddPaisToVacunas(idVacuna, con);
        //        }
        //        return vacunas;
        //    }
        //    catch (Exception e)
        //    {
        //        System.Diagnostics.Debug.Assert(false, "Error al listar vacunas por país de origen del laboratorio" + e.Message);
        //        return null;
        //    }
        //    finally
        //    {
        //        manejadorConexion.CerrarConexion(con);
        //    }

        //}

        //public IEnumerable<Vacuna> FindAllByTypeVac(string idTipo)
        //{
        //    Conexion manejadorConexion = new Conexion();
        //    SqlConnection con = manejadorConexion.CrearConexion();

        //    try
        //    {
        //        SqlCommand query = new SqlCommand("SELECT * FROM Vacunas " +
        //                                          "WHERE IdTipo = (SELECT Id FROM TipoVacunas WHERE Descripcion=@idTipo)", con);
        //        manejadorConexion.AbrirConexion(con);

        //        query.Parameters.AddWithValue("@idTipo", idTipo);
        //        SqlDataReader dataReader = query.ExecuteReader();

        //        List<Vacuna> vacunas = new List<Vacuna>();

        //        while (dataReader.Read())
        //        {
        //            int idVacuna = (int)dataReader["Id"];
        //            string idTipoDB = (string)dataReader["IdTipo"];
        //            Vacuna unaVacuna = new Vacuna()
        //            {
        //                Id = idVacuna,
        //                Nombre = (string)dataReader["Nombre"],
        //                Precio = (decimal)dataReader["Precio"],

        //            };
        //            vacunas.Add(unaVacuna);
        //            unaVacuna.ListaLaboratorios = AddLabsToVacunas(idVacuna, con);
        //            unaVacuna.TipoVacuna = AddTipoVacunaToVacunas(idTipoDB, con);
        //            unaVacuna.ListaPaises = AddPaisToVacunas(idVacuna, con);
        //        }
        //        return vacunas;
        //    }
        //    catch (Exception e)
        //    {
        //        System.Diagnostics.Debug.Assert(false, "Error al listar vacunas por identificador de tipo de vacuna" + e.Message);
        //        return null;
        //    }
        //    finally
        //    {
        //        manejadorConexion.CerrarConexion(con);
        //    }

        //}

        //public IEnumerable<Vacuna> FindAllByLabName(string nombreLab)
        //{
        //    Conexion manejadorConexion = new Conexion();
        //    SqlConnection con = manejadorConexion.CrearConexion();

        //    try
        //    {
        //        SqlCommand query = new SqlCommand("SELECT * FROM Vacunas " +
        //                                          "WHERE Id IN (SELECT IdVacuna " +
        //                                                        "FROM VacunaLaboratorios " +
        //                                                        "WHERE IdLaboratorio IN (SELECT Id " +
        //                                                                                "FROM Laboratorios " +
        //                                                                                "WHERE Nombre like @Nombre))", con);
        //        manejadorConexion.AbrirConexion(con);

        //        query.Parameters.AddWithValue("@Nombre", nombreLab + "%");
        //        SqlDataReader dataReader = query.ExecuteReader();

        //        List<Vacuna> vacunas = new List<Vacuna>();

        //        while (dataReader.Read())
        //        {
        //            int idVacuna = (int)dataReader["Id"];
        //            string idTipo = (string)dataReader["IdTipo"];
        //            Vacuna unaVacuna = new Vacuna()
        //            {
        //                Id = idVacuna,
        //                Nombre = (string)dataReader["Nombre"],
        //                IdTipo = idTipo,
        //                Precio = (decimal)dataReader["Precio"],

        //            };
        //            vacunas.Add(unaVacuna);
        //            unaVacuna.ListaLaboratorios = AddLabsToVacunas(idVacuna, con);
        //            unaVacuna.TipoVacuna = AddTipoVacunaToVacunas(idTipo, con);
        //            unaVacuna.ListaPaises = AddPaisToVacunas(idVacuna, con);
        //        }
        //        return vacunas;
        //    }
        //    catch (Exception e)
        //    {
        //        System.Diagnostics.Debug.Assert(false, "Error al listar vacunas por nombre de laboratorio" + e.Message);
        //        return null;
        //    }
        //    finally
        //    {
        //        manejadorConexion.CerrarConexion(con);
        //    }
        //}

        //public IEnumerable<Vacuna> FindAllByMaxPrice(decimal precioMax)
        //{
        //    Conexion manejadorConexion = new Conexion();
        //    SqlConnection con = manejadorConexion.CrearConexion();

        //    try
        //    {
        //        SqlCommand query = new SqlCommand("Select * from Vacunas where Precio <= @Precio", con);
        //        manejadorConexion.AbrirConexion(con);

        //        query.Parameters.AddWithValue("@Precio", precioMax);
        //        SqlDataReader dataReader = query.ExecuteReader();

        //        List<Vacuna> vacunas = new List<Vacuna>();

        //        while (dataReader.Read())
        //        {
        //            int idVacuna = (int)dataReader["Id"];
        //            string idTipo = (string)dataReader["IdTipo"];
        //            Vacuna unaVacuna = new Vacuna()
        //            {
        //                Id = idVacuna,
        //                Nombre = (string)dataReader["Nombre"],
        //                IdTipo = idTipo,
        //                Precio = (decimal)dataReader["Precio"],

        //            };
        //            vacunas.Add(unaVacuna);
        //            unaVacuna.ListaLaboratorios = AddLabsToVacunas(idVacuna, con);
        //            unaVacuna.TipoVacuna = AddTipoVacunaToVacunas(idTipo, con);
        //            unaVacuna.ListaPaises = AddPaisToVacunas(idVacuna, con);
        //        }
        //        return vacunas;
        //    }
        //    catch (Exception e)
        //    {
        //        System.Diagnostics.Debug.Assert(false, "Error al listar vacunas por tope superior de precio" + e.Message);
        //        return null;
        //    }
        //    finally
        //    {
        //        manejadorConexion.CerrarConexion(con);
        //    }

        //}

        //public IEnumerable<Vacuna> FindAllByMinPrice(decimal precioMin)
        //{
        //    Conexion manejadorConexion = new Conexion();
        //    SqlConnection con = manejadorConexion.CrearConexion();

        //    try
        //    {
        //        SqlCommand query = new SqlCommand("Select * from vacunas where Precio >= @Precio", con);
        //        manejadorConexion.AbrirConexion(con);

        //        query.Parameters.AddWithValue("@Precio", precioMin);
        //        SqlDataReader dataReader = query.ExecuteReader();

        //        List<Vacuna> vacunas = new List<Vacuna>();

        //        while (dataReader.Read())
        //        {
        //            int idVacuna = (int)dataReader["Id"];
        //            string idTipo = (string)dataReader["IdTipo"];
        //            Vacuna unaVacuna = new Vacuna()
        //            {
        //                Id = idVacuna,
        //                Nombre = (string)dataReader["Nombre"],
        //                IdTipo = idTipo,
        //                Precio = (decimal)dataReader["Precio"],

        //            };
        //            vacunas.Add(unaVacuna);
        //            unaVacuna.ListaLaboratorios = AddLabsToVacunas(idVacuna, con);
        //            unaVacuna.TipoVacuna = AddTipoVacunaToVacunas(idTipo, con);
        //            unaVacuna.ListaPaises = AddPaisToVacunas(idVacuna, con);
        //        }
        //        return vacunas;
        //    }
        //    catch (Exception e)
        //    {
        //        System.Diagnostics.Debug.Assert(false, "Error al listar vacunas por tope inferior de precio" + e.Message);
        //        return null;
        //    }
        //    finally
        //    {
        //        manejadorConexion.CerrarConexion(con);
        //    }
        //}

        //public bool Update(Vacuna unaVacuna)
        //{
        //    if (unaVacuna == null) return false;

        //    Conexion manejadorConexion = new Conexion();
        //    SqlConnection con = manejadorConexion.CrearConexion();

        //    try
        //    {
        //        SqlCommand query = new SqlCommand("UPDATE Vacunas SET FaseClinicaAprob=@FaseClinicaAprob, " +
        //                                                                "Precio=@Precio, " +
        //                                                                "IdUsuario=@IdUsuario, " +
        //                                                                "UltimaModificacion=@UltimaModificacion " +
        //                                                                "WHERE Id=@Id", con);
        //        manejadorConexion.AbrirConexion(con);

        //        query.Parameters.AddWithValue("@Id", unaVacuna.Id);
        //        query.Parameters.AddWithValue("@IdUsuario", unaVacuna.IdUsuario);
        //        query.Parameters.AddWithValue("@FaseClinicaAprob", unaVacuna.FaseClinicaAprob);
        //        query.Parameters.AddWithValue("@Precio", unaVacuna.ValidatePrice(unaVacuna));
        //        query.Parameters.AddWithValue("@UltimaModificacion", DateTime.Now);
        //        SqlDataReader dataReader = query.ExecuteReader();
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        System.Diagnostics.Debug.Assert(false, "Error al editar una vacuna" + e.Message);
        //        return false;
        //    }
        //    finally
        //    {
        //        manejadorConexion.CerrarConexion(con);
        //    }
        //}
    }
}
