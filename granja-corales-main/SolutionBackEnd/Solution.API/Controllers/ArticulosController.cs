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
  
    public class ArticulosController : ControllerBase
    {
        private readonly SolutionDbContext _context;
        //Declaracion del automapper para poder caster los objetos 
        private readonly IMapper _mapper;

        public ArticulosController(SolutionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Articulos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<datamodels.Articulos>>> GetArticulos()
        {
            
            var aux = await new Solution.BS.Articulos(_context).GetAllWithAsync();

            var mapaux = _mapper.Map<IEnumerable<data.Articulos>, IEnumerable<datamodels.Articulos>>(aux).ToList();
            return mapaux;
          
        }

        // GET: api/Articulos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<datamodels.Articulos>> GetArticulos(int id)
        {
            var Articulos = await new Solution.BS.Articulos(_context).GetOneByIdWithAsync(id);
            var mapaux = _mapper.Map<data.Articulos, datamodels.Articulos>(Articulos);

            if (mapaux == null)
            {
                return NotFound();
            }

            return mapaux;
        }

        // PUT: api/Articulos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticulos(int id, datamodels.Articulos Articulos)
        {
            if (id != Articulos.IdArticulo)
            {
                return BadRequest();
            }

            try
            {
                var mapaux = _mapper.Map<datamodels.Articulos, data.Articulos>(Articulos);

                new Solution.BS.Articulos(_context).Update(mapaux);
            }
            catch (Exception ee)
            {
                if (!ArticulosExists(id))
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

        // POST: api/Articulos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<datamodels.Articulos>> PostArticulos(datamodels.Articulos Articulos)
        {
            var mapaux = _mapper.Map<datamodels.Articulos, data.Articulos>(Articulos);

            new Solution.BS.Articulos(_context).Insert(mapaux);

            return CreatedAtAction("GetArticulos", new { id = Articulos.IdArticulo }, Articulos);
        }

        // DELETE: api/Articulos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<datamodels.Articulos>> DeletArticulos(int id)
        {
            var Articulos = new Solution.BS.Articulos(_context).GetOneById(id);
            if (Articulos == null)
            {
                return NotFound();
            }

            new Solution.BS.Articulos(_context).Delete(Articulos);
            var mapaux = _mapper.Map<data.Articulos, datamodels.Articulos>(Articulos);


            return mapaux;
        }

        private bool ArticulosExists(int id)
        {
            return (new Solution.BS.Articulos(_context).GetOneById(id) != null);
        }
    }
}
