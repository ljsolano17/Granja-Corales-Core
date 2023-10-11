using Solution.DAL.EF;
using Solution.DO.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using data = Solution.DO.Objects;
using Microsoft.EntityFrameworkCore;

namespace Solution.DAL.Repository
{
  
    public class RepositoryArticulosSolicitud : Repository<data.ArticulosSolicitud>, IRepositoryArticulosSolicitud
    {
       
        public RepositoryArticulosSolicitud(SolutionDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<ArticulosSolicitud>> GetAllWithAsAsync()
        {
            return await _db.ArticulosSolicitud
                .Include(m => m.IdArticuloNavigation)
                .Include(m=>m.IdSolicitudNavigation)
                .ToListAsync();
        }

        public async Task<ArticulosSolicitud> GetByOneWithAsAsync(int id)
        {
            return await _db.ArticulosSolicitud
              .Include(m => m.IdArticuloNavigation)
                .Include(m => m.IdSolicitudNavigation)
             .SingleOrDefaultAsync(m => m.IdArticuloSolicitud == id);
        }

       
        private SolutionDbContext _db
        {
            get { return dbContext as SolutionDbContext; }
        }
    }
}
