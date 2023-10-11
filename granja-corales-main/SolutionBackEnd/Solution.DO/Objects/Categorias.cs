using System;
using System.Collections.Generic;
using System.Text;

namespace Solution.DO.Objects
{
    public class Categorias
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
