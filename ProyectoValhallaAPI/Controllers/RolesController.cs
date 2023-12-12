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
    public class RolesController : ControllerBase
    {
        private readonly ValhallaDbContext _context;

        public RolesController(ValhallaDbContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolDTO>>> GetRoles()
        {
          if (_context.Roles == null)
          {
              return NotFound();
          }
            var response = await _context.Roles.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { Msg = "Okey", Response = response });
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RolDTO>> GetRole(int id)
        {
          if (_context.Roles == null)
          {
              return NotFound();
          }
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status200OK, new { Msg = "Okey", Response = role });
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, Role role)
        {
            if (id != role.IdRol)
            {
                return BadRequest();
            }

            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
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

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RolDTO>> PostRole(RolDTO rol)
        {
          if (_context.Roles == null)
          {
              return Problem("Entity set 'ValhallaDbContext.Roles'  is null.");
          }

            var rolDTO = new Role
            {

                NombreRol = rol.NombreRol,
                Descripcion = rol.Descripcion,
                
            };

            _context.Roles.Add(rolDTO);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { Msg = "Creacion Exitosa" }); ;
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            if (_context.Roles == null)
            {
                return NotFound();
            }
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { Msg = "Eliminacion Exitosa" });
        }

        private bool RoleExists(int id)
        {
            return (_context.Roles?.Any(e => e.IdRol == id)).GetValueOrDefault();
        }
    }
}
