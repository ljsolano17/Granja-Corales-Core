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
   
    public class CategoriasController : ControllerBase
    {
        private readonly SolutionDbContext _context;
       
        private readonly IMapper _mapper;

        public CategoriasController(SolutionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<datamodels.Categorias>>> GetCategorias()
        {
            // Declaramos una variable para traer la informacion
            var aux = new Solution.BS.Categorias(_context).GetAll();

            var mapaux = _mapper.Map<IEnumerable<data.Categorias>, IEnumerable<datamodels.Categorias>>(aux).ToList();
            return mapaux;

        }

        // GET: api/Categorias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<datamodels.Categorias>> GetCategorias(int id)
        {
            var Categorias = new Solution.BS.Categorias(_context).GetOneById(id);

            if (Categorias == null)
            {
                return NotFound();
            }
            var mapaux = _mapper.Map<data.Categorias, datamodels.Categorias>(Categorias);
            return mapaux;
        }

        // PUT: api/Categorias/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategorias(int id, datamodels.Categorias Categorias)
        {
            if (id != Categorias.IdCategoria)
            {
                return BadRequest();
            }

            try
            {
                var mapaux = _mapper.Map<datamodels.Categorias, data.Categorias>(Categorias);
                new Solution.BS.Categorias(_context).Update(mapaux);
            }
            catch (Exception ee)
            {
                if (!CategoriasExists(id))
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

        // POST: api/Categorias
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<datamodels.Categorias>> PostCategorias(datamodels.Categorias Categorias)
        {
            var mapaux = _mapper.Map<datamodels.Categorias, data.Categorias>(Categorias);
            new Solution.BS.Categorias(_context).Insert(mapaux);

            return CreatedAtAction("GetCategorias", new { id = Categorias.IdCategoria }, Categorias);
        }

        // DELETE: api/Categorias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<datamodels.Categorias>> DeleteCategorias(int id)
        {
            var Categorias = new Solution.BS.Categorias(_context).GetOneById(id);
            if (Categorias == null)
            {
                return NotFound();
            }

            new Solution.BS.Categorias(_context).Delete(Categorias);
            var mapaux = _mapper.Map<data.Categorias, datamodels.Categorias>(Categorias);

            return mapaux;
        }

        private bool CategoriasExists(int id)
        {
            return (new Solution.BS.Categorias(_context).GetOneById(id) != null);
        }
    }
}
