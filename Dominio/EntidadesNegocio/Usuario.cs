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
        public string Password { get; set; }

        public Usuario() { }

        public static string GenerarPassword(string documento)
        {
            string[] dias = new string[7] { "Lun", "Mar", "Mier", "Jue", "Vie", "Sab", "Dom" };
            return documento.Substring(0, 4) + "-" + dias[(int)new DateTime().DayOfWeek - 1];
        }
    }
}
