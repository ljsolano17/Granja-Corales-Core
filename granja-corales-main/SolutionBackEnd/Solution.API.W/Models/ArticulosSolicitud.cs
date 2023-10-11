using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Solution.API.W.Models
{
    public partial class ArticulosSolicitud
    {
        public int IdArticuloSolicitud { get; set; }
        public int IdSolicitud { get; set; }
        public int IdArticulo { get; set; }

        public virtual Articulos IdArticuloNavigation { get; set; }
        public virtual Solicitudes IdSolicitudNavigation { get; set; }
    }
}
