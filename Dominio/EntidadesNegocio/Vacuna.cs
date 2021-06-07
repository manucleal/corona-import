using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.EntidadesNegocio
{
    [Table("vacunas")]
    public class Vacuna
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La cantidad dosis es obligatorio")]
        [Range(1, 10, ErrorMessage = "El valor {0} debe estar entre {1} y {2}.")]
        public int CantidadDosis { get; set; }

        [Required(ErrorMessage = "El lapso días es obligatorio")]
        [Range(0, 300, ErrorMessage = "El valor {0} debe estar entre {1} y {2}.")]
        public int LapsoDiasDosis { get; set; }

        [Required(ErrorMessage = "El Min Edad es obligatorio")]
        [Range(0, 120, ErrorMessage = "El valor {0} debe estar entre {1} y {2}.")]
        public int MinEdad { get; set; }

        [Required(ErrorMessage = "El Max Edad es obligatorio")]
        [Range(0, 120, ErrorMessage = "El valor {0} debe estar entre {1} y {2}.")]
        public int MaxEdad { get; set; }

        [Required(ErrorMessage = "La Eficacía. prev es obligatorio")]
        [Range(0, 100, ErrorMessage = "El valor {0} debe estar entre {1} y {2}.")]
        public int EficaciaPrev { get; set; }

        [Required(ErrorMessage = "La Efic. hos es obligatorio")]
        [Range(0, 100, ErrorMessage = "El valor {0} debe estar entre {1} y {2}.")]
        public int EficaciaHosp { get; set; }

        [Required(ErrorMessage = "La Efic. CTI es obligatorio")]
        [Range(0, 100, ErrorMessage = "El valor {0} debe estar entre {1} y {2}.")]
        public int EficaciaCti { get; set; }

        [Required(ErrorMessage = "La Min temp. es obligatorio")]
        [Range(-100, 50, ErrorMessage = "El valor {0} debe estar entre {1} y {2}.")]
        public int MinTemp { get; set; }

        [Required(ErrorMessage = "La Max temp. es obligatorio")]
        [Range(-100, 50, ErrorMessage = "El valor {0} debe estar entre {1} y {2}.")]
        public int MaxTemp { get; set; }

        [Required(ErrorMessage = "La Produccion anual es obligatorio")]
        [Range(1, 9999000000000000, ErrorMessage = "El valor {0} debe estar entre {1} y {2}.")]
        public long ProduccionAnual { get; set; }

        [Required(ErrorMessage = "La Fase Clínica aprob. es obligatorio")]
        [Range(1, 4, ErrorMessage = "El valor {0} debe estar entre {1} y {2}.")]
        public int FaseClinicaAprob { get; set; }

        [StringLength(200, ErrorMessage = "Solamente estan permitidos 100 caracteres")]
        public string EfectosAdversos { get; set; }

        public bool Emergencia { get; set; }
        public decimal Precio { get; set; }
        public DateTime UltimaModificacion { get; set; }
        public string IdUsuario { get; set; }

        public string IdTipo { get; set; }
        public TipoVacuna TipoVacuna { get; set; }

        public string[] Paises { get; set; }
        //public int[] Laboratorios { get; set; }
        public ICollection<Laboratorio> Laboratorios { get; set; } = new List<Laboratorio>();
        //public ICollection<Pais> ListaPaises { get; set; } = new List<Pais>();
        public bool Covax { get; set; }

        public Vacuna() { }
    }
}
