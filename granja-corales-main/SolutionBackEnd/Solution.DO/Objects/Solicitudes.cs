using System;
using System.Collections.Generic;
using System.Text;

namespace Solution.DO.Objects
{
    public class Solicitudes
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
