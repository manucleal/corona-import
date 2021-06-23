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
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    return dataBase.Laboratorios.ToList();
                }
            }
            catch (Exception exp) { return null; }
        }
    }
}
