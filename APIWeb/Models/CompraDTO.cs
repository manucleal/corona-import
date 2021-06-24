using Dominio.EntidadesNegocio;
using System.Text.Json

    namespace APIWeb.Models
{
    public class CompraDTO
    {
        public MutualistaDTO Mutualista { get; set; }
        public VacunaDTO Vacuna { get; set; }
        public int CantidadDosis { get; set; }
        public decimal Monto { get; set; }

        public CompraDTO() { }

        public static CompraVacuna MapearACompraVacuna(CompraDTO compra)
        {
            if (compra == null)
                return null;
            return new CompraVacuna
            {
                Mutualista = MutualistaDTO.MapearAMutualista(compra.Mutualista),
                Vacuna = VacunaDTO.MapearAVacuna(compra.Vacuna),
                CantidadDosis = compra.CantidadDosis,
                Monto = compra.Monto
            };
        }
    }

    public class MutualistaDTO
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string NombreContacto { get; set; }
        public int TopeComprasMensuales { get; set; }
        public int CantidadAfiliados { get; set; }
        public int MontoMaxVacunasPorAfiliado { get; set; }
        public int ComprasRealizadas { get; set; }

        public MutualistaDTO() { }

        public static Mutualista MapearAMutualista(MutualistaDTO mutualista)
        {
            if (mutualista == null)
                return null;
            return new Mutualista
            {
                Codigo = mutualista.Codigo,
                Nombre = mutualista.Nombre,
                Telefono = mutualista.Telefono,
                NombreContacto = mutualista.NombreContacto,
                TopeComprasMensuales = mutualista.TopeComprasMensuales,
                CantidadAfiliados = mutualista.CantidadAfiliados,
                MontoMaxVacunasPorAfiliado = mutualista.MontoMaxVacunasPorAfiliado,
                ComprasRealizadas = mutualista.ComprasRealizadas,
            };
        }
    }

    public class VacunaDTO
    {
        public int Id { get; set; }
       public string Nombre { get; set; }
        public int CantidadDosis { get; set; }
        public int LapsoDiasDosis { get; set; }
        public int MinEdad { get; set; }
        public int MaxEdad { get; set; }
        public int EficaciaPrev { get; set; }
        public int EficaciaHosp { get; set; }
        public int EficaciaCti { get; set; }
        public int MinTemp { get; set; }
        public int MaxTemp { get; set; }
        public long ProduccionAnual { get; set; }
        public int FaseClinicaAprob { get; set; }
        public string EfectosAdversos { get; set; }
        public bool Emergencia { get; set; }
        public decimal Precio { get; set; }
        public string Documento { get; set; }
        public TipoVacuna Tipo { get; set; }
        public string Paises { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public bool Covax { get; set; }

        public VacunaDTO() { }

        public static Vacuna MapearAVacuna(VacunaDTO vacuna)
        {
            if (vacuna == null)
                return null;
            return new Vacuna
            {
                Id = vacuna.Id,
                Nombre = vacuna.Nombre,
                CantidadDosis = vacuna.CantidadDosis,
                LapsoDiasDosis = vacuna.LapsoDiasDosis,
                MaxEdad = vacuna.MaxEdad,
                MinEdad = vacuna.MinEdad,
                EficaciaPrev = vacuna.EficaciaPrev,
                EficaciaHosp = vacuna.EficaciaHosp,
                EficaciaCti = vacuna.EficaciaCti,
                MinTemp = vacuna.MinTemp,
                MaxTemp = vacuna.MaxTemp,
                ProduccionAnual = vacuna.ProduccionAnual,
                FaseClinicaAprob = vacuna.FaseClinicaAprob,
                Emergencia = vacuna.Emergencia,
                Precio = vacuna.Precio,
                Documento = vacuna.Documento,
                Tipo = vacuna.Tipo,
                Paises = vacuna.Paises,
                Covax = vacuna.Covax,
                EfectosAdversos = vacuna.EfectosAdversos
            };
        }
    }
}