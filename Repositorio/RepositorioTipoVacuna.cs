using System;
using System.Collections.Generic;
using System.Linq;
using AccesoDatos.Contexto;
using Dominio.EntidadesNegocio;
using Dominio.InterfacesRepositorio;

namespace Repositorio
{
    public class RepositorioTipoVacuna : IRepositorioTipoVacuna
    {

        public bool Add(TipoVacuna tipoVacuna)
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    dataBase.TipoVacunas.Add(tipoVacuna);
                    dataBase.SaveChanges();
                }
                return true;
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Assert(false, "Error al ingresar Tipo de Vacunas" + exp.Message);
                return false;
            }
        }

        public TipoVacuna FindById(string id)
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    var resultado = dataBase.TipoVacunas
                        .Where(t => t.Id == id).FirstOrDefault();

                    return resultado;
                }
            }
            catch (Exception exp)
            {
                System.Diagnostics.Debug.Assert(false, "Error al obtener Laboratorio" + exp.Message);
                return null;
            }
        }

        public IEnumerable<TipoVacuna> FindAll()
        {
            try
            {
                using (CoronaImportContext dataBase = new CoronaImportContext())
                {
                    return dataBase.TipoVacunas.ToList();
                }
            }
            catch (Exception exp) { return null; }
        }
    }
}
