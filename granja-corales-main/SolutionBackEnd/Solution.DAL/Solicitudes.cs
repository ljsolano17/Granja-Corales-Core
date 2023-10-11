using Solution.DAL.EF;
using Solution.DAL.Repository;
using Solution.DO.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using data = Solution.DO.Objects;
namespace Solution.DAL
{
    public class Solicitudes : ICRUD<data.Solicitudes>
    {
        private Repository<data.Solicitudes> _repo = null;
        public Solicitudes(SolutionDbContext solutionDbContext)
        {
            _repo = new Repository<data.Solicitudes>(solutionDbContext);
        }
        public void Delete(data.Solicitudes t)
        {
            _repo.Delete(t);
            _repo.Commit();
        }

        public IEnumerable<data.Solicitudes> GetAll()
        {
            return _repo.GetAll();
        }

        public data.Solicitudes GetOneById(int id)
        {
            return _repo.GetOneById(id);
        }

        public void Insert(data.Solicitudes t)
        {
            _repo.Insert(t);
            _repo.Commit();
        }

        public void Update(data.Solicitudes t)
        {
            _repo.Update(t);
            _repo.Commit();
        }
    }
}
