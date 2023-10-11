using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using data = Solution.DO.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Solution.DAL.EF;
using AutoMapper;
using datamodels = Solution.API.DataModels;

namespace Solution.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ArticulosSolicitudController : ControllerBase
    {
        private readonly SolutionDbContext _context;
       
        private readonly IMapper _mapper;

        public ArticulosSolicitudController(SolutionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ArticulosSolicitud
        [HttpGet]
        public async Task<ActionResult<IEnumerable<datamodels.ArticulosSolicitud>>> GetArticulosSolicitud()
        {
           
            var aux = await new Solution.BS.ArticulosSolicitud(_context).GetAllWithAsync();

            var mapaux = _mapper.Map<IEnumerable<data.ArticulosSolicitud>, IEnumerable<datamodels.ArticulosSolicitud>>(aux).ToList();
            return mapaux;
            

        }

        // GET: api/ArticulosSolicitud/5
        [HttpGet("{id}")]
        public async Task<ActionResult<datamodels.ArticulosSolicitud>> GetArticulosSolicitud(int id)
        {
            var ArticulosSolicitud = await new Solution.BS.ArticulosSolicitud(_context).GetOneByIdWithAsync(id);
            var mapaux = _mapper.Map<data.ArticulosSolicitud, datamodels.ArticulosSolicitud>(ArticulosSolicitud);

            if (mapaux == null)
            {
                return NotFound();
            }

            return mapaux;
        }

        // PUT: api/ArticulosSolicitud/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticulosSolicitud(int id, datamodels.ArticulosSolicitud ArticulosSolicitud)
        {
            if (id != ArticulosSolicitud.IdArticuloSolicitud)
            {
                return BadRequest();
            }

            try
            {
                var mapaux = _mapper.Map<datamodels.ArticulosSolicitud, data.ArticulosSolicitud>(ArticulosSolicitud);

                new Solution.BS.ArticulosSolicitud(_context).Update(mapaux);
            }
            catch (Exception ee)
            {
                if (!ArticulosSolicitudExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ArticulosSolicitud
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<datamodels.ArticulosSolicitud>> PostArticulosSolicitud(datamodels.ArticulosSolicitud ArticulosSolicitud)
        {
            var mapaux = _mapper.Map<datamodels.ArticulosSolicitud, data.ArticulosSolicitud>(ArticulosSolicitud);

            new Solution.BS.ArticulosSolicitud(_context).Insert(mapaux);

            return CreatedAtAction("GetArticulosSolicitud", new { id = ArticulosSolicitud.IdArticuloSolicitud }, ArticulosSolicitud);
        }

        // DELETE: api/ArticulosSolicitud/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<datamodels.ArticulosSolicitud>> DeleteArticulosSolicitud(int id)
        {
            var ArticulosSolicitud = new Solution.BS.ArticulosSolicitud(_context).GetOneById(id);
            if (ArticulosSolicitud == null)
            {
                return NotFound();
            }

            new Solution.BS.ArticulosSolicitud(_context).Delete(ArticulosSolicitud);
            var mapaux = _mapper.Map<data.ArticulosSolicitud, datamodels.ArticulosSolicitud>(ArticulosSolicitud);


            return mapaux;
        }

        private bool ArticulosSolicitudExists(int id)
        {
            return (new Solution.BS.ArticulosSolicitud(_context).GetOneById(id) != null);
        }
    }
}
