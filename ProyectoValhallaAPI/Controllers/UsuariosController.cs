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
    public class UsuariosController : ControllerBase
    {
        private readonly ValhallaDbContext _context;

        public UsuariosController(ValhallaDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
        {
          if (_context.Usuarios == null)
          {
              return NotFound();
          }
            var response = await _context.Usuarios.Include(R => R.IdRolNavigation).ToListAsync();
            return StatusCode(StatusCodes.Status200OK, new { Msg = "Okey", Response = response});

        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(int id)
        {
          if (_context.Usuarios == null)
          {
              return NotFound();
          }
            var usuario = await _context.Usuarios.Include(u => u.IdRolNavigation).FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status200OK, new { Msg = "Okey", Objeto = usuario});
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioDTO usuario)
        {


            if (id != usuario.IdUsuario)
            {
                return BadRequest();
            }

            var usuarioDTO = new Usuario
            {
                IdUsuario = usuario.IdUsuario,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                CorreoElectronico = usuario.CorreoElectronico,
                Username = usuario.Username,
                Telefono = usuario.Telefono,
                IdRol = usuario.IdRol,
                Password = usuario.Password,
            };

            _context.Entry(usuarioDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(StatusCodes.Status200OK, new { Msg = "Actualizacion Exitosa"});
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> PostUsuario(UsuarioDTO usuario)
        {
          if (_context.Usuarios == null)
          {
              return Problem("Entity set 'ValhallaDbContext.Usuarios'  is null.");
          }

            var usuarioDTO = new Usuario
            {
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                CorreoElectronico = usuario.CorreoElectronico,
                Username = usuario.Username,
                Telefono = usuario.Telefono,
                IdRol = usuario.IdRol,
                Password = usuario.Password,     
            };

            _context.Usuarios.Add(usuarioDTO);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { Msg = "Creacion Exitosa"});
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK, new { Msg = "Eliminacion Exitosa"});
        }

        private bool UsuarioExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.IdUsuario == id)).GetValueOrDefault();
        }
    }
}
