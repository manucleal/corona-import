using System;
using Repositorio;
using System.Web.Http;

namespace APIWeb.Controllers
{
    [RoutePrefix("api/vacunas")]
    public class VacunaController : ApiController
    {
        private RepositorioVacuna repositorioVacuna = new RepositorioVacuna();

        [HttpGet]
        [Route("filters/all")]
        public IHttpActionResult GetFiltersAND([FromUri]int faseClinicaAprob = -1, [FromUri]int precioMin = -1, [FromUri]int precioMax = -1, [FromUri]string tipo = "", [FromUri]string laboratorio = "", [FromUri]string paisAceptada = "")
        {
            try
            {            
                var vacunas = repositorioVacuna.FindAllByFiltersAND(faseClinicaAprob, precioMin, precioMax, tipo, laboratorio, paisAceptada);
                return Ok(vacunas);
            }
            catch (Exception exp)
            {
                return InternalServerError (new Exception("No es posible filtrar las vacunas", exp.InnerException));
            }
        }

        [HttpGet]
        [Route("filters/any")]
        public IHttpActionResult GetFiltersOR([FromUri]int faseClinicaAprob = -1, [FromUri]int precioMin = -1, [FromUri]int precioMax = -1, [FromUri]string tipo = "", [FromUri]string laboratorio = "", [FromUri]string paisAceptada = "")
        {
            try
            {
                var vacunas = repositorioVacuna.FindAllByFiltersOR(faseClinicaAprob, precioMin, precioMax, tipo, laboratorio, paisAceptada);
                return Ok(vacunas);
            }
            catch (Exception exp)
            {
                return InternalServerError(new Exception("No es posible filtrar las vacunas", exp.InnerException));
            }
        }
    }
}
