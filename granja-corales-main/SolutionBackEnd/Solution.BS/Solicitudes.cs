using Solution.DAL.EF;
using Solution.DO.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using data = Solution.DO.Objects;
namespace Solution.BS
{
    public class Solicitudes : ICRUD<data.Solicitudes>
    {
        private SolutionDbContext context;
        public Solicitudes(SolutionDbContext _context)
        {
            context = _context;
        }
        public void Delete(data.Solicitudes t)
        {
            new DAL.Solicitudes(context).Delete(t);
        }

        public IEnumerable<data.Solicitudes> GetAll()
        {
            return new DAL.Solicitudes(context).GetAll();
        }

        public data.Solicitudes GetOneById(int id)
        {
            return new DAL.Solicitudes(context).GetOneById(id);
        }

        public void Insert(data.Solicitudes t)
        {
            new DAL.Solicitudes(context).Insert(t);
        }

        public void Update(data.Solicitudes t)
        {
            new DAL.Solicitudes(context).Update(t);
        }
    }
}
