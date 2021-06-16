using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIWeb.Controllers
{
    [RoutePrefix("api/vacunas")]
    public class VacunaController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IEnumerable<string> Get()
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
