using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Solution.DAL.EF;
using Solution.DAL.Repository;
using Solution.DO.Interfaces;
using data = Solution.DO.Objects;

namespace Solution.DAL
{
    public class ArticulosSolicitud : ICRUD<data.ArticulosSolicitud>
    {

        private RepositoryArticulosSolicitud _repo = null;

        public ArticulosSolicitud(SolutionDbContext solutionDbContext)
        {
            _repo = new RepositoryArticulosSolicitud(solutionDbContext);
        }

        public void Delete(data.ArticulosSolicitud t)
        {
            _repo.Delete(t);
            _repo.Commit();
        }

        public IEnumerable<data.ArticulosSolicitud> GetAll()
        {
            return _repo.GetAll();
        }

        public data.ArticulosSolicitud GetOneById(int id)
        {
            return _repo.GetOneById(id);
        }

        public void Insert(data.ArticulosSolicitud t)
        {
            _repo.Insert(t);
            _repo.Commit();
        }

        public void Update(data.ArticulosSolicitud t)
        {
            _repo.Update(t);
            _repo.Commit();
        }

        public async Task<IEnumerable<data.ArticulosSolicitud>> GetAllWithAsync()
        {
            return await _repo.GetAllWithAsAsync();
        }

        public async Task<data.ArticulosSolicitud> GetOneByIdWithAsync(int id)
        {
            return await _repo.GetByOneWithAsAsync(id);
        }
    }
}
