using System;
using System.Collections.Generic;
using Repositorio;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIWeb.Controllers
{
    [RoutePrefix("api/vacunas")]
    public class VacunaController : ApiController
    {
        private RepositorioVacuna repositorioVacuna = new RepositorioVacuna();

        [HttpGet]
        [Route("filters")]

        public IHttpActionResult Get([FromUri]int faseClinicaAprob = -1, [FromUri]int precioMin = -1, [FromUri]int precioMax = -1, [FromUri]string tipo = "", [FromUri]string laboratorio = "", [FromUri]string paisAceptada = "")
        {
            //return new string[] {
            //    "faseClinicaAprob", faseClinicaAprob.ToString(),
            //    "PrecioMin", precioMin.ToString(),
            //    "PrecioMax", precioMax.ToString(),
            //    "tipo", tipo.ToString(),
            //    "laboratorio", laboratorio.ToString(),
            //    "paisAceptada", paisAceptada.ToString(),
            //};
            try
            {            
                var vacunas = repositorioVacuna.FindAllByFilters(faseClinicaAprob, precioMin, precioMax, tipo, laboratorio, paisAceptada);
                return Ok(vacunas);
            }
            catch (Exception exp)
            {                
                return null;
            }
        }

        [HttpGet]
        [Route ("faseAprobacion/{faseDeAprobacion:int}")]
        public IHttpActionResult GetVacunasPorFaseAprobacion(int faseDeAprobacion)
        {
            return Ok();
        }

        [HttpGet]
        [Route("precio/min/{precioMin:int}/max/{precioMax:int}")]
        public IHttpActionResult GetVacunasPorRangoPrecios(decimal precioMinm, decimal precioMax)
        {
            return Ok();
        }

        [HttpGet]
        [Route("tipo/{tipo}")]
        public IHttpActionResult GetVacunasPorTipo(string tipo)
        {
            return Ok();
        }

        [HttpGet]
        [Route("laboratorio/{laboratorio}")]
        public IHttpActionResult GetVacunasPorLaboratorio(string laboratorio)
        {
            return Ok();
        }

        [HttpGet]
        [Route("pais/{pais}")]
        public IHttpActionResult GetVacunasPorPais(string pais)
        {
            return Ok();
        }
    }
}
