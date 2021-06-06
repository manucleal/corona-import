using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dominio.EntidadesNegocio;
using AccesoDatos.Contexto;
using Importador;

namespace AplicacionWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            //Usuario usuario = new Usuario() { Id = 1, Nombre = "Emanuel" };
            //using (CoronaImportContext db = new CoronaImportContext())
            //{
            //    db.Usuarios.Add(usuario);
            //    db.SaveChanges();
            //}
            ManejadorArchivo.ObtenerUsuarios();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}