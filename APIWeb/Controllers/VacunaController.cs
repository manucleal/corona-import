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
//        public IEnumerable<string> Get([FromUri]int faseClinicaAprob, int PrecioMin, int PrecioMax, string tipo, string laboratorio, string paisAceptada)

        public IEnumerable<string> Get([FromUri]int faseClinicaAprob, [FromUri]int PrecioMin, [FromUri]int PrecioMax, [FromUri]string tipo, [FromUri]string laboratorio, [FromUri]string paisAceptada)
        {
            return new string[] { "value1", "value2" };
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
