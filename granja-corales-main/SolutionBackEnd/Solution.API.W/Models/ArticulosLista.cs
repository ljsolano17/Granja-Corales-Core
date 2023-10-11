using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Solution.API.W.Models
{
    public partial class ArticulosLista
    {
        public int IdArticulosLista { get; set; }
        public int IdArticulo { get; set; }
        public int IdUsuario { get; set; }

        public virtual Articulos IdArticuloNavigation { get; set; }
    }
}
