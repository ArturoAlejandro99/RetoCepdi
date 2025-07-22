using Microsoft.AspNetCore.Mvc;
using RetoTecnico.API.DTOs;
using RetoTecnico.API.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace RetoTecnico.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetUsuarios([FromQuery] UsuarioQueryParamsDTO queryParams)
        {
            var usuariosDto = await _usuarioService.GetAllUsuariosAsync(queryParams);
            if (!usuariosDto.Any())
            {
                return NotFound("No se encontraron usuarios.");
            }
            return Ok(usuariosDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponseDto>> GetUsuario(int id)
        {
            var usuarioDto = await _usuarioService.GetUsuarioByIdAsync(id);
            if (usuarioDto == null)
            {
                return NotFound($"Usuario con ID {id} no encontrado.");
            }
            return Ok(usuarioDto);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioResponseDto>> PostUsuario(UsuarioCreateDto usuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdUserDto = await _usuarioService.CreateUsuarioAsync(usuarioDto);
                return CreatedAtAction("GetUsuario", new { id = createdUserDto.Idusuario }, createdUserDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioCreateDto usuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var isUpdated = await _usuarioService.UpdateUsuarioAsync(id, usuarioDto);
                if (!isUpdated)
                {
                    return NotFound($"Usuario con ID {id} no encontrado para actualizar.");
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var isDeleted = await _usuarioService.DeleteUsuarioAsync(id);
            if (!isDeleted)
            {
                return NotFound($"Usuario con ID {id} no encontrado para eliminar.");
            }

            return NoContent();
        }
    }
}