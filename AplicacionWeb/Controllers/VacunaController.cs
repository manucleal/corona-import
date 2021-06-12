using System;
using System.Web.Mvc;
//using Repositorios;
using Dominio.EntidadesNegocio;
//using WebApplication.ReferenciaServicioVacunas;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Controllers
{
    public class VacunaController : Controller
    {
        private ServicioVacunasClient serviciosVacunas = new ServicioVacunasClient();
        private RepositorioVacuna repoVacuna = new RepositorioVacuna();
        private RepositorioLaboratorio repoLaboratorio = new RepositorioLaboratorio();
        private RepositorioTipoVacuna repoTipoVacuna = new RepositorioTipoVacuna();
        private RepositorioPais repoPais = new RepositorioPais();

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<DtoVacunas> vacunas = serviciosVacunas.GetTodasLasVacunas();
            ViewBag.Vacunas = vacunas;
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

            IEnumerable<DtoVacunas> vacunas = serviciosVacunas.GetTodasLasVacunas();
            ViewBag.Vacunas = vacunas;
            return View();
        }

        [HttpGet]
        public ActionResult Modificar(int id)
        {
            if ((string)Session["documento"] == null && (string)Session["nombre"] == null)
            {
                Session["documento"] = null;
                Session["nombre"] = null;
                return RedirectToAction("Index", "Vacuna");
            }
            Vacuna vacuna = repoVacuna.FindById(id);
            ViewBag.H1 = "";
            return View(vacuna);
        }

        [HttpPost]
        public ActionResult Modificar(Vacuna unaVauna)
        {
            unaVauna.IdUsuario = (string)Session["documento"];
            if (repoVacuna.Update(unaVauna))
            {
                return RedirectToAction("IndexAuth", "Vacuna");
            }
            return View();
        }

        [HttpPost]
        public ActionResult IndexAuth(string tipoFiltro, string filtroText = "", int filtroNumber = 0)
        {
            if (tipoFiltro != null && (filtroText != "" || filtroNumber >= 0))
            {
                switch (tipoFiltro)
                {
                    case "PorNombre":
                        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorNombre(filtroText);
                        break;
                    case "PorFaseAprob":
                        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorFaseAprob(filtroNumber);
                        break;
                    case "PorPaisLab":
                        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorPaisLab(filtroText);
                        break;
                    case "PorTipoVac":
                        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorTipoVac(filtroText);
                        break;
                    case "PorTopeInferior":
                        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorTopeInferior(filtroNumber);
                        break;
                    case "PorTopeSuperior":
                        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorTopeSuperior(filtroNumber);
                        break;
                    case "PorNombreLab":
                        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunasPorNombreLab(filtroText);
                        break;
                    default:
                        ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunas();
                        break;
                }
            }
            else
            {
                ViewBag.Vacunas = serviciosVacunas.GetTodasLasVacunas();
            }

            return View("IndexAuth");
        }

        [HttpGet]
        public ActionResult Alta()
        {
            if ((string)Session["documento"] == null && (string)Session["nombre"] == null) {
                Session["documento"] = null;
                Session["nombre"] = null;
                return RedirectToAction("Index", "Vacuna");
            }

            cargarFiltros();
            return View();
        }

        [HttpPost]
        public ActionResult Alta(Vacuna unaVacuna)
        {
            unaVacuna.IdUsuario = (string)Session["documento"];                
            IEnumerable<Vacuna> vacunas = repoVacuna.FindAllByName(unaVacuna.Nombre);

            if (vacunas.ToList().Count == 0)
            {
                if (unaVacuna.ValidateTemperature(unaVacuna))
                {
                    if (unaVacuna.ValidateAge(unaVacuna))
                    {
                        if (unaVacuna.ValidateCantidadDosis(unaVacuna))
                        {
                            if (unaVacuna.ValidateProduccionAnual(unaVacuna))
                            {
                                if (repoVacuna.Add(unaVacuna)) return RedirectToAction("Index", "Vacuna");
                            }
                            else
                            {
                                ModelState.AddModelError("produccionAnual", "Debe ser positivo");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("cantidadDosis", "Debe ser positivo");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("minEdad", "Debe ser menor o igual a Máxima edad.");
                    }
                }
                else
                {
                    ModelState.AddModelError("minTemp", "Debe ser menor o igual a Máxima temp.");
                }
            } else
            {
                ModelState.AddModelError("nombre", "Ya existe una vacuna con ese nombre");
            }
            cargarFiltros();
            return View(unaVacuna);
        }

        public void cargarFiltros()
        {            
            ViewBag.Laboratorios = repoLaboratorio.FindAll();            
            ViewBag.TipoVacunas = repoTipoVacuna.FindAll();            
            ViewBag.Paises = repoPais.FindAll();
        }
    }
}