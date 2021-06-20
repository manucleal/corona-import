using System;
using System.Web.Mvc;
//using Repositorios;
using Dominio.EntidadesNegocio;
//using WebApplication.ReferenciaServicioVacunas;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApplication.Controllers
{
    public class VacunaController : Controller
    {
        HttpClient cliente = new HttpClient();
        HttpResponseMessage response = new HttpResponseMessage();
        Uri vacunaUri = null;

        public VacunaController()
        {
            cliente.BaseAddress = new Uri("http://localhost:49340");
            vacunaUri = new Uri("http://localhost:49340/api/vacunas");
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public ActionResult Index()
        {
            //IEnumerable<DtoVacunas> vacunas = serviciosVacunas.GetTodasLasVacunas();
            //ViewBag.Vacunas = vacunas;
            return View();
        }

        [HttpGet]
        public ActionResult IndexAuth()
        {
            if ((string)Session["documento"] == null && (string)Session["nombre"] == null)
            {
                Session["documento"] = null;
                Session["nombre"] = null;
                return RedirectToAction("Index", "Vacuna");
            }

            //response = cliente.GetAsync(vacunaUri).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var vacunas = response.Content.ReadAsAsync<IEnumerable<Models.>>().Result;
            //    if (vacunas != null && vacunas.Count() > 0)
            //        return View("IndexAuth", vacunas.ToList());
            //    else
            //    {
            //        TempData["ResultadoOperacion"] = "No hay vacunas disponibles";
            //        return View("IndexAuth", new List<ProductoModel>());
            //    }
            //}
            //else
            //{
            //    TempData["ResultadoOperacion"] = "Error desconocido";
            //    return View("IndexAuth");
            //}
            //IEnumerable<DtoVacunas> vacunas = serviciosVacunas.GetTodasLasVacunas();
            //ViewBag.Vacunas = vacunas;
            return View();
        }

        [HttpPost]
        public ActionResult IndexAuth(string tipoFiltro, string filtroText = "", int filtroNumber = 0)
        {
            if (tipoFiltro != null && (filtroText != "" || filtroNumber >= 0))
            {
                //switch (tipoFiltro)
                //{
                //    case "PorNombre":
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorNombre(filtroText);
                //        break;
                //    case "PorFaseAprob":
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorFaseAprob(filtroNumber);
                //        break;
                //    case "PorPaisLab":
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorPaisLab(filtroText);
                //        break;
                //    case "PorTipoVac":
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorTipoVac(filtroText);
                //        break;
                //    case "PorTopeInferior":
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorTopeInferior(filtroNumber);
                //        break;
                //    case "PorTopeSuperior":
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorTopeSuperior(filtroNumber);
                //        break;
                //    case "PorNombreLab":
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorNombreLab(filtroText);
                //        break;
                //    default:
                //        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunas();
                //        break;
                //}
            }
            else
            {
                //ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunas();
            }

            return View("IndexAuth");
        }

        public void cargarFiltros()
        {            
            //ViewBag.Laboratorios = repoLaboratorio.FindAll();            
            //ViewBag.TipoVacunas = repoTipoVacuna.FindAll();            
            //ViewBag.Paises = repoPais.FindAll();
        }
    }
}