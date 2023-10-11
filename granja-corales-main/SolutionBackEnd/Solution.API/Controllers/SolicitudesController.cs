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
  
    public class SolicitudesController : ControllerBase
    {
        private readonly SolutionDbContext _context;
       
        private readonly IMapper _mapper;

        public SolicitudesController(SolutionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Solicitudes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<datamodels.Solicitudes>>> GetSolicitudes()
        {
            // Declaramos una variable para traer la informacion
            var aux = new Solution.BS.Solicitudes(_context).GetAll();

            var mapaux = _mapper.Map<IEnumerable<data.Solicitudes>, IEnumerable<datamodels.Solicitudes>>(aux).ToList();
            return mapaux;

        }

        // GET: api/Solicitudes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<datamodels.Solicitudes>> GetSolicitudes(int id)
        {
            var Solicitudes = new Solution.BS.Solicitudes(_context).GetOneById(id);

            if (Solicitudes == null)
            {
                return NotFound();
            }
            var mapaux = _mapper.Map<data.Solicitudes, datamodels.Solicitudes>(Solicitudes);
            return mapaux;
        }

        // PUT: api/Solicitudes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitudes(int id, datamodels.Solicitudes Solicitudes)
        {
            if (id != Solicitudes.IdSolicitud)
            {
                return BadRequest();
            }

            try
            {
                var mapaux = _mapper.Map<datamodels.Solicitudes, data.Solicitudes>(Solicitudes);
                new Solution.BS.Solicitudes(_context).Update(mapaux);
            }
            catch (Exception ee)
            {
                if (!SolicitudesExists(id))
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

        // POST: api/Solicitudes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<datamodels.Solicitudes>> PostSolicitudes(datamodels.Solicitudes Solicitudes)
        {
            var mapaux = _mapper.Map<datamodels.Solicitudes, data.Solicitudes>(Solicitudes);
            new Solution.BS.Solicitudes(_context).Insert(mapaux);

            return CreatedAtAction("GetSolicitudes", new { id = Solicitudes.IdSolicitud }, Solicitudes);
        }

        // DELETE: api/Solicitudes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<datamodels.Solicitudes>> DeleteSolicitudes(int id)
        {
            var Solicitudes = new Solution.BS.Solicitudes(_context).GetOneById(id);
            if (Solicitudes == null)
            {
                return NotFound();
            }

            new Solution.BS.Solicitudes(_context).Delete(Solicitudes);
            var mapaux = _mapper.Map<data.Solicitudes, datamodels.Solicitudes>(Solicitudes);

            return mapaux;
        }

        private bool SolicitudesExists(int id)
        {
            return (new Solution.BS.Solicitudes(_context).GetOneById(id) != null);
        }
    }
}
