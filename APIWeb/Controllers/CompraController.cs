using System;
using Repositorio;
using System.Web.Http;

namespace APIWeb.Controllers
{
    [RoutePrefix("api/compra/mutualista")]
    public class CompraController : ApiController
    {

        private RepositorioCompraVacuna repositorioCompraVacuna = new RepositorioCompraVacuna();
        private RepositorioVacuna repositorioVacuna = new RepositorioVacuna();

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetComprasMutualista([FromUri]int id = -1)
        {
            try
            {
                var vacunas = repositorioCompraVacuna.FindAllByMutualista(id);
                return Ok(vacunas);
            }
            catch (Exception exp)
            {
                return InternalServerError(new Exception("No es posible obtener las compras", exp.InnerException));
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult GuardarCompraMutualista([FromBody]Models.CompraDTO datos)
        {
            try
            {
                var vacuna = repositorioVacuna.FindById(datos.IdVacuna);
                if (vacuna != null) {
                    datos.Vacuna = Models.VacunaDTO.MapearAVacunaDTO(vacuna);
                    var compra = Models.CompraDTO.MapearACompraVacuna(datos);
                    if (repositorioCompraVacuna.Add(compra))
                    {
                        return Ok();
                    }
                    else
                    {
                        return InternalServerError(new Exception("No es posible guardar la compra"));
                    }
                }
                else
                {
                    return (BadRequest());
                }
 
            }
            catch (Exception exp)
            {
                return InternalServerError(new Exception("No es posible guardar la compra", exp.InnerException));
            }
        }
        
    }
}
