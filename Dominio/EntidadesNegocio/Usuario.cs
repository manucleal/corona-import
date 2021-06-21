using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.EntidadesNegocio
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        [Required(ErrorMessage = "El documento es obligatorio")]
        [MinLength(7, ErrorMessage = "Documento incorrecto")]
        [MaxLength(8, ErrorMessage = "Documento incorrecto")]
        public string Documento { get; set; }
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El password es obligatorio")]
        [MinLength(6, ErrorMessage = "Debe superar 6 caracteres")]
        public string Password { get; set; }
        public int CantidadLogin { get; set; } = 0;

        public Usuario() { }

        public static string EncodePasswordToBase64(string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
            return Convert.ToBase64String(inArray);
        }

        public static bool VerificoPass(string password)
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

        public static DateTime GenerarDateTime(string datos)
        {
            if (datos != null )
            {
                string[] fechaHora = datos.Split(' ');
                string[] fecha = fechaHora[1].Split('/');
                string[] hora = fechaHora[2].Split(':');
                return new DateTime(int.Parse(fecha[2]), int.Parse(fecha[1]), int.Parse(fecha[0]), int.Parse(hora[0]), int.Parse(hora[1]), int.Parse(hora[2]));
            }
            return new DateTime();
        }
    }
}
