using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using data = Solution.DO.Objects;
using Microsoft.EntityFrameworkCore;
using Solution.DAL.EF;
using Solution.DO.Objects;

namespace Solution.DAL.Repository
{

    public class RepositoryArticulos : Repository<data.Articulos>, IRepositoryArticulos
    {
        public RepositoryArticulos(SolutionDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Articulos>> GetAllWithAsAsync()
        {
            return await _db.Articulos
                .Include(m => m.IdCategoriaNavigation)
                .ToListAsync();
        }

        public async Task<Articulos> GetByOneWithAsAsync(int id)
        {
            return await _db.Articulos
             .Include(m => m.IdCategoriaNavigation)
             .SingleOrDefaultAsync(m => m.IdArticulo == id);
        }

     
        private SolutionDbContext _db
        {
            get { return dbContext as SolutionDbContext; }
        }
    }

}
