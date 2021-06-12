using System.Web.Mvc;
using Dominio.EntidadesNegocio;
using Repositorio;

namespace WebApplication.Controllers
{
    public class UsuarioController : Controller
    {
        public ActionResult ResetPassword()
        {
            if ((string)Session["documento"] != null && Session["nombre"] != null)
            {
                return RedirectToAction("IndexAuth", "Vacuna");
            }
            return View("ResetPassword");
        }

        [HttpPost]
        public ActionResult ResetPassword(Usuario unUsuario)
        {
            if (unUsuario.VerificoPass(unUsuario.Password))
            {
                RepositorioUsuario repoUsuario = new RepositorioUsuario();
                Usuario usuario = repoUsuario.FindById(unUsuario.Documento);

                if (usuario.Documento != null)
                {
                    ModelState.AddModelError("documento", "El documento ya está registrado");
                }
                else if (repoUsuario.Add(unUsuario))
                {
                    Session["documento"] = unUsuario.Documento;
                    Session["nombre"] = usuario.Nombre;
                    return RedirectToAction("IndexAuth", "Vacuna");
                }
                else
                {
                    View("ResetPassword");
                }
            }
            else
            {
                ModelState.AddModelError("password", "Contraseña débil");
            }

            return View("Login");
        }

        public ActionResult Login()
        {
            if ((string)Session["documento"] != null && (string)Session["nombre"] != null){
                return RedirectToAction("IndexAuth", "Vacuna");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(Usuario unUsuario)
        { 
            if (ModelState.IsValid)
            {
                if (unUsuario.VerificoPass(unUsuario.Password))
                {
                    RepositorioUsuario repoUsuario = new RepositorioUsuario();
                    Usuario usuario = repoUsuario.Login(unUsuario);

                    if (usuario.Documento != null)
                    {
                        Session["documento"] = usuario.Documento;
                        Session["nombre"] = usuario.Nombre;
                        return RedirectToAction("IndexAuth", "Vacuna");
                    }
                    else
                    {
                        ModelState.AddModelError("password", "Documento y/o contraseña incorrectos");
                    }
                }
                else
                {
                    ModelState.AddModelError("password", "Contraseña débil");
                }
            }

            return View();
        }

        public ActionResult Salir()
        {
            Session["documento"] = null;
            Session["nombre"] = null;
            return RedirectToAction("Index", "Vacuna");
        }
    }
}