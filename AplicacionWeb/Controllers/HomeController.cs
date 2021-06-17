﻿using System.Web.Mvc;
using AccesoDatos.Contexto;
using Dominio.EntidadesNegocio;
using Importador;

namespace AplicacionWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Create()
        {
            Usuario usuario = new Usuario() { Documento = "46902781", Nombre = "Emanuel", Password = "Emanuel" };
            using (CoronaImportContext db = new CoronaImportContext())
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
            }
            return View();
        }

        public ActionResult Importador()
        {
            ManejadorArchivo.ImportarDatos();
            return View();
        }
    }
}