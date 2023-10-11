using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Solution.API.W.Model
{
    public partial class Solicitudes
    {
        public Solicitudes()
        {
            ArticulosSolicitud = new HashSet<ArticulosSolicitud>();
        }

        public int IdSolicitud { get; set; }
        public DateTime FecCreacion { get; set; }
        public string EstadoSolicitud { get; set; }
        public string IdUsuario { get; set; }
        public string EstadoAprobacion { get; set; }

        public virtual ICollection<ArticulosSolicitud> ArticulosSolicitud { get; set; }
    }
}
