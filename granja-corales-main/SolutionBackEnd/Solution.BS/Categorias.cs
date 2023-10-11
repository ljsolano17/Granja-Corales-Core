using Solution.DAL.EF;
using Solution.DO.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using data = Solution.DO.Objects;

namespace Solution.BS
{
  
    public class Categorias : ICRUD<data.Categorias>
    {
        private SolutionDbContext context;
        public Categorias(SolutionDbContext _context)
        {
            context = _context;
        }
        public void Delete(data.Categorias t)
        {
            new DAL.Categorias(context).Delete(t);
        }

        public IEnumerable<data.Categorias> GetAll()
        {
            return new DAL.Categorias(context).GetAll();
        }

        public data.Categorias GetOneById(int id)
        {
            return new DAL.Categorias(context).GetOneById(id);
        }

        public void Insert(data.Categorias t)
        {
            new DAL.Categorias(context).Insert(t);
        }

        public void Update(data.Categorias t)
        {
            new DAL.Categorias(context).Update(t);
        }
    }
}
