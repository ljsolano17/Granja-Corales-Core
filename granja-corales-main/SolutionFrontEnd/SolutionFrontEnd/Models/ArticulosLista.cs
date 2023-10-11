using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SolutionFrontEnd.Models
{
    public partial class ArticulosLista
    {
        public int Id { get; set; }
        public Articulos articulo { get; set; }
        public int Cantidad { get; set; }

    }
}
