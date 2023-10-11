using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Solution.API.W.ModelsNuevo
{
    public partial class Articulos
    {
        public Articulos()
        {
            ArticulosSolicitud = new HashSet<ArticulosSolicitud>();
        }

        public int IdArticulo { get; set; }
        public string NombreCientifico { get; set; }
        public string Familia { get; set; }
        public string Tipo { get; set; }
        public string NombreComun { get; set; }
        public string Dificultad { get; set; }
        public string Temperamento { get; set; }
        public string Color { get; set; }
        public string Dieta { get; set; }
        public string TamanoMax { get; set; }
        public string Origen { get; set; }
        public string TamanoMinPecera { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FecModificacion { get; set; }
        public int? IdCategoria { get; set; }

        public virtual Categorias IdCategoriaNavigation { get; set; }
        public virtual ICollection<ArticulosSolicitud> ArticulosSolicitud { get; set; }
    }
}
