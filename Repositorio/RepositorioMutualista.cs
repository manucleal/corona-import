using System;
using System.Collections.Generic;
using System.Linq;
using AccesoDatos.Contexto;
using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorio;

namespace AccesoDatos.Repositorios
{
    public class RepositorioMutualista : IRepositorioMutualista
    {
        public bool Add(Mutualista unaMutualista)
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    dataBase.Mutualistas.Add(unaMutualista);
                    dataBase.SaveChanges();
                }
                return true;
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Assert(false, "Error al ingresar Mutualista" + exp.Message);
                return false;
            }
        }

        public Mutualista FindById(int id)
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    var resultado = dataBase.Mutualistas
                        .Where(m => m.Id == id).FirstOrDefault();

                    return resultado;
                }
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Assert(false, "Error al obtener Mutualista" + exp.Message);
                return null;
            }
        }

        public Mutualista FindByName(string nombre)
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    var resultado = dataBase.Mutualistas
                        .Where(m => m.Nombre == nombre).FirstOrDefault();

                    return resultado;
                }
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Assert(false, "Error al obtener Mutualista por nombre" + exp.Message);
                return null;
            }
        }

        public IEnumerable<Mutualista> FindAll()
        {
            try
            {
                using (CoronaImportContext db = new CoronaImportContext())
                {
                     var resultado = db.Mutualistas.ToList();
                    return resultado;
                }
            }
            catch (Exception ex) { return null; }
        }

        public decimal CalcularMontoTotalCompras (int id)
        {
            try
            {
                using (var db = new CoronaImportContext())
                {
                    var total =
                        db.CompraVacunas
                        .Where(c => c.Mutualista.Id == id)
                        .Sum(c => c.Monto);
                    return total;
                }
            }
            catch (Exception ex) { return -1; }
        }
    }
}
