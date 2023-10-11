using Solution.DAL.EF;
using Solution.DAL.Repository;
using Solution.DO.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using data = Solution.DO.Objects;

namespace Solution.DAL
{
    public class Categorias : ICRUD<data.Categorias>
    {
        private Repository<data.Categorias> _repo = null;
        public Categorias(SolutionDbContext solutionDbContext)
        {
            _repo = new Repository<data.Categorias>(solutionDbContext);
        }
        public void Delete(data.Categorias t)
        {
            _repo.Delete(t);
            _repo.Commit();
        }

        public IEnumerable<data.Categorias> GetAll()
        {
            return _repo.GetAll();
        }

        public data.Categorias GetOneById(int id)
        {
            return _repo.GetOneById(id);
        }

        public void Insert(data.Categorias t)
        {
            _repo.Insert(t);
            _repo.Commit();
        }

        public void Update(data.Categorias t)
        {
            _repo.Update(t);
            _repo.Commit();
        }
    }
}
