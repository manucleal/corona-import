using System;
using System.Web.Mvc;
using Dominio.EntidadesNegocio;
using System.Collections.Generic;
using System.Linq;
using AccesoDatos.Repositorios;
using Repositorio;
using AplicacionWeb.Models;

namespace WebApplication.Controllers
{
    public class VacunaController : Controller
    {

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

        //TODO: poner enlace en cada vacuna que me lleve a este get
        [HttpGet]
        public ActionResult CompraVacuna()
        {
            RepositorioMutualista repoMutualista = new RepositorioMutualista();
            RepositorioVacuna repoVacuna = new RepositorioVacuna();

            if ((string)Session["documento"] != null && Session["nombre"] != null)
            {
                ViewModelVacuna viewModelVacuna = ViewModelVacuna.MapearAViewModelVacuna(repoVacuna.FindById(2)) ;
                if (viewModelVacuna != null)
                {
                    ViewBag.Mutualistas = repoMutualista.FindAll();
                    return View(viewModelVacuna);
                }
            }
            return RedirectToAction("Login", "Usuario");
        }

        //hacer post CompraVacuna para capturar idMutualista y cantVacunas
        [HttpPost]
        public ActionResult CompraVacuna(int? IdVac, int? Mutualista, ViewModelVacuna vacuna)
        {
            return View();
        }
    }
}