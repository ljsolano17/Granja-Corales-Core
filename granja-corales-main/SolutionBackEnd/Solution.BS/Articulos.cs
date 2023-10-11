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

    public class Articulos : ICRUD<data.Articulos>
    {
        private SolutionDbContext _repo = null;

        public Articulos(SolutionDbContext solutionDbContext)
        {
            _repo = solutionDbContext;
        }

        public void Delete(data.Articulos t)
        {
            new DAL.Articulos(_repo).Delete(t);
        }

        public IEnumerable<data.Articulos> GetAll()
        {
            return new DAL.Articulos(_repo).GetAll();
        }

        public data.Articulos GetOneById(int id)
        {
            return new DAL.Articulos(_repo).GetOneById(id);
        }

        public void Insert(data.Articulos t)
        {
            new DAL.Articulos(_repo).Insert(t);
        }

        public void Update(data.Articulos t)
        {
            new DAL.Articulos(_repo).Update(t);
        }

        public async Task<IEnumerable<data.Articulos>> GetAllWithAsync()
        {
            return await new DAL.Articulos(_repo).GetAllWithAsync();
        }

        public async Task<data.Articulos> GetOneByIdWithAsync(int id)
        {
            return await new DAL.Articulos(_repo).GetOneByIdWithAsync(id);
        }
    }
}
