using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
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
    public class RolesPermisosController : ControllerBase
    {
        private readonly ValhallaDbContext _context;

        public RolesPermisosController(ValhallaDbContext context)
        {
            _context = context;
        }

        // GET: api/RolesPermisoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolesPermiso>>> GetRolesPermisos()
        {
          if (_context.RolesPermisos == null)
          {
              return NotFound();
          }
            var response = await _context.RolesPermisos.Include(R => R.IdPermisoNavigation).Include(R => R.IdRolNavigation).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { Msg = "Okey", Response = response });
        }

        // GET: api/RolesPermisoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RolesPermiso>> GetRolesPermiso(int id)
        {
          if (_context.RolesPermisos == null)
          {
              return NotFound();
          }
            var rolesPermiso = await _context.RolesPermisos.FindAsync(id);

            if (rolesPermiso == null)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status200OK, new { Msg = "Okey", Response = rolesPermiso });
        }

        // PUT: api/RolesPermisoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRolesPermiso(int id, PermisosRolesDTO rolesPermiso)
        {
            if (id != rolesPermiso.IdRolPermiso)
            {
                return BadRequest("Rol No Encontrado");
            }

            _context.Entry(rolesPermiso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolesPermisoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(StatusCodes.Status200OK, new { Msg = "Actualizacion Exitosa" });
        }

        // POST: api/RolesPermisoes
        [HttpPost]
        public async Task<ActionResult<PermisosRolesDTO>> PostRolesPermiso(PermisosRolesDTO rolesPermiso)
        {
          if (_context.RolesPermisos == null)
          {
              return Problem("Entity set 'ValhallaDbContext.RolesPermisos'  is null.");
          }

            var rolesPermisoDTO = new RolesPermiso
            {
                IdPermiso = rolesPermiso.IdPermiso,
                IdRol = rolesPermiso.IdRol,
            };

            _context.RolesPermisos.Add(rolesPermisoDTO);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { Msg = "Creacion Exitosa" });
        }

        // DELETE: api/RolesPermisoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRolesPermiso(int id)
        {
            if (_context.RolesPermisos == null)
            {
                return NotFound();
            }
            var rolesPermiso = await _context.RolesPermisos.FindAsync(id);
            if (rolesPermiso == null)
            {
                return NotFound();
            }

            _context.RolesPermisos.Remove(rolesPermiso);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { Msg = "Eliminacion Exitosa"});
        }

        private bool RolesPermisoExists(int id)
        {
            return (_context.RolesPermisos?.Any(e => e.IdRolPermiso == id)).GetValueOrDefault();
        }
    }
}
