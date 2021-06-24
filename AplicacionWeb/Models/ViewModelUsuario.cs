using Dominio.EntidadesNegocio;

namespace AplicacionWeb.Models
{
    public class ViewModelUsuario
    {
        public ViewModelUsuario() { }

        public string Documento { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public static Usuario MapearAUsuario(ViewModelUsuario viewModelUsuario)
        {
            if (viewModelUsuario == null)
                return null;
            return new Usuario
            {
                Documento = viewModelUsuario.Documento,
                Password = viewModelUsuario.Password
            };
        }
    }
}