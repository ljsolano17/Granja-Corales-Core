using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Solution.DAL.EF;
using Solution.DAL.Repository;
using Solution.DO.Interfaces;
using data = Solution.DO.Objects;

namespace Solution.BS
{

    public class ArticulosSolicitud : ICRUD<data.ArticulosSolicitud>
    {
        private SolutionDbContext _repo = null;

        public ArticulosSolicitud(SolutionDbContext solutionDbContext)
        {
            _repo = solutionDbContext;
        }

        public void Delete(data.ArticulosSolicitud t)
        {
            new DAL.ArticulosSolicitud(_repo).Delete(t);
        }

        public IEnumerable<data.ArticulosSolicitud> GetAll()
        {
            return new DAL.ArticulosSolicitud(_repo).GetAll();
        }

        public data.ArticulosSolicitud GetOneById(int id)
        {
            return new DAL.ArticulosSolicitud(_repo).GetOneById(id);
        }

        public void Insert(data.ArticulosSolicitud t)
        {
            new DAL.ArticulosSolicitud(_repo).Insert(t);
        }

        public void Update(data.ArticulosSolicitud t)
        {
            new DAL.ArticulosSolicitud(_repo).Update(t);
        }

        public async Task<IEnumerable<data.ArticulosSolicitud>> GetAllWithAsync()
        {
            return await new DAL.ArticulosSolicitud(_repo).GetAllWithAsync();
        }

        public async Task<data.ArticulosSolicitud> GetOneByIdWithAsync(int id)
        {
            return await new DAL.ArticulosSolicitud(_repo).GetOneByIdWithAsync(id);
        }
    }
}
