using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.EntidadesNegocio
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }

        public Usuario() { }
      
        public bool VerificoPass(string password)
        {
            int contMay = 0;
            int contMin = 0;
            int contDig = 0;
            if (password.Length >= 6)
            {
                for (int i = 0; i < password.Length; i++)
                {
                    if (char.IsUpper(password[i]))
                    {
                        contMay++;
                    }
                    if (char.IsLower(password[i]))
                    {
                        contMin++;
                    }
                    if (char.IsNumber(password[i]))
                    {
                        contDig++;
                    }
                }
            }
            return (contMay > 0 && contMin > 0 && contDig > 0);
        }

        public static string GenerarPassword(string documento)
        {
            string[] dias = new string[7] { "Lun", "Mar", "Mier", "Jue", "Vie", "Sab", "Dom" };
            return documento.Substring(0, 4) + "-" + dias[(int)new DateTime().DayOfWeek - 1];
        }
    }
}
