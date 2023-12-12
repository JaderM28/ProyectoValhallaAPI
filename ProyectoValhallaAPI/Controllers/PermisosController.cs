using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoValhallaAPI.Models;
using ProyectoValhallaAPI.Models.ModelDTO;

namespace ProyectoValhallaAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly ValhallaDbContext _context;

        public PermisosController(ValhallaDbContext context)
        {
            _context = context;
        }

        // GET: api/Permisoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermisoDTO>>> GetPermisos()
        {
          if (_context.Permisos == null)
          {
              return NotFound();
          }
            var response = await _context.Permisos.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { Msg = "Okey", Response = response });

        }

        // GET: api/Permisoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PermisoDTO>> GetPermiso(int id)
        {
          if (_context.Permisos == null)
          {
              return NotFound();
          }
            var permiso = await _context.Permisos.FindAsync(id);

            if (permiso == null)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status200OK, new { Msg = "Okey", Response = permiso});
        }

        // PUT: api/Permisoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermiso(int id, PermisoDTO permiso)
        {
            if (id != permiso.IdPermiso)
            {
                return BadRequest();
            }

            _context.Entry(permiso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermisoExists(id))
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

        // POST: api/Permisoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PermisoDTO>> PostPermiso(PermisoDTO permiso)
        {
          if (_context.Permisos == null)
          {
              return Problem("Entity set 'ValhallaDbContext.Permisos'  is null.");
          }

            var permisoDTO = new Permiso
            {
                NombreModulo = permiso.NombreModulo,
                NombrePermiso = permiso.NombrePermiso,
            };


            _context.Permisos.Add(permisoDTO);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { Msg = "Creacion Exitosa" }); ;
        }

        // DELETE: api/Permisoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermiso(int id)
        {
            if (_context.Permisos == null)
            {
                return NotFound();
            }
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }

            _context.Permisos.Remove(permiso);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { Msg = "Eliminacion Exitosa" });
        }

        private bool PermisoExists(int id)
        {
            return (_context.Permisos?.Any(e => e.IdPermiso == id)).GetValueOrDefault();
        }
    }
}
