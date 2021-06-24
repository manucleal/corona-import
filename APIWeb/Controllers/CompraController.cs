using System;
using Repositorio;
using System.Web.Http;

namespace APIWeb.Controllers
{
    [RoutePrefix("api/compra/mutualista")]
    public class CompraController : ApiController
    {

        private RepositorioCompraVacuna repositorioCompraVacuna = new RepositorioCompraVacuna();

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
                if (repositorioCompraVacuna.Add(Models.CompraDTO.MapearACompraVacuna(datos)))
                {
                    return Ok();
                } else
                {
                    return InternalServerError(new Exception("No es posible guardar la compra"));
                }
            }
            catch (Exception exp)
            {
                return InternalServerError(new Exception("No es posible guardar la compra", exp.InnerException));
            }
        }
        
    }
}
