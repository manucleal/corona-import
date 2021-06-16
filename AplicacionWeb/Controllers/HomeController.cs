using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Importador;

namespace AplicacionWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Create()
        {
            //Usuario usuario = new Usuario() { Documento = "46902781", Nombre = "Emanuel", Password = "Emanuel" };
            //using (CoronaImportContext db = new CoronaImportContext())
            //{
            //    db.Usuarios.Add(usuario);
            //    db.SaveChanges();
            //}
            ManejadorArchivo.ImportarDatos();
            return View();
        }
    }
}