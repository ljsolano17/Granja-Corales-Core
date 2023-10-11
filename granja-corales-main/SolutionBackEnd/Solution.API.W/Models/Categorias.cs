using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Solution.API.W.Models
{
    public partial class Categorias
    {
        public Categorias()
        {
            Articulos = new HashSet<Articulos>();
        }

        public int IdCategoria { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<Articulos> Articulos { get; set; }
    }
}
