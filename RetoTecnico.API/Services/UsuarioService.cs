using Microsoft.EntityFrameworkCore;
using RetoTecnico.API.DTOs;
using RetoTecnico.API.Models;
using RetoTecnico.API.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using System;

namespace RetoTecnico.API.Services;

public class UsuarioService : IUsuarioService
{
    private readonly CepdiPruebaContext _context;

    public UsuarioService(CepdiPruebaContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<UsuarioResponseDto> Data, int TotalRecords)> GetAllUsuariosAsync(UsuarioQueryParamsDTO queryParams)
    {
        var query = _context.Usuarios.AsQueryable();

        if (!string.IsNullOrEmpty(queryParams.SearchTerm))
        {
            query = query.Where(u =>
                u.Nombre != null && u.Nombre.Contains(queryParams.SearchTerm) ||
                u.NombreUsuario != null && u.NombreUsuario.Contains(queryParams.SearchTerm)
            );
        }

        var totalRecords = await query.CountAsync();

        if (!string.IsNullOrEmpty(queryParams.SortBy))
        {
            switch (queryParams.SortBy.ToLower())
            {
                case "nombre":
                    query = query.OrderBy(u => u.Nombre);
                    break;
                case "nombreusuario":
                    query = query.OrderBy(u => u.NombreUsuario);
                    break;
                case "fechacreacion":
                    query = query.OrderBy(u => u.Fechacreacion);
                    break;
                default:
                    query = query.OrderBy(u => u.Idusuario);
                    break;
            }
            if (queryParams.SortOrder != null && queryParams.SortOrder.ToLower() == "desc")
            {
                query = query.Reverse();
            }
        }
        else
        {
            query = query.OrderBy(u => u.Idusuario);
        }

        var usuarios = await query
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .ToListAsync();

        var usuarioDtos = usuarios.Select(u => new UsuarioResponseDto
            {
                Idusuario = u.Idusuario,
                Nombre = u.Nombre,
                Fechacreacion = u.Fechacreacion,
                NombreUsuario = u.NombreUsuario,
                Idperfil = u.Idperfil,
                Estatus = u.Estatus
            }).ToList();

            return (usuarioDtos, totalRecords);
    }

    public async Task<UsuarioResponseDto?> GetUsuarioByIdAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return null;
        }

        return new UsuarioResponseDto
        {
            Idusuario = usuario.Idusuario,
            Nombre = usuario.Nombre,
            Fechacreacion = usuario.Fechacreacion,
            NombreUsuario = usuario.NombreUsuario,
            Idperfil = usuario.Idperfil,
            Estatus = usuario.Estatus
        };
    }

    public async Task<UsuarioResponseDto> CreateUsuarioAsync(UsuarioCreateDto usuarioDto)
    {
        var existingUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == usuarioDto.NombreUsuario);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"El nombre de usuario '{usuarioDto.NombreUsuario}' ya existe.");
        }

        var usuario = new Usuario
        {
            Nombre = usuarioDto.Nombre,
            NombreUsuario = usuarioDto.NombreUsuario,
            Password = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Password),
            Idperfil = usuarioDto.Idperfil,
            Estatus = 1,
            Fechacreacion = DateTime.Now
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return new UsuarioResponseDto
        {
            Idusuario = usuario.Idusuario,
            Nombre = usuario.Nombre,
            Fechacreacion = usuario.Fechacreacion,
            NombreUsuario = usuario.NombreUsuario,
            Idperfil = usuario.Idperfil,
            Estatus = usuario.Estatus
        };
    }

    public async Task<bool> UpdateUsuarioAsync(int id, UsuarioCreateDto usuarioDto)
    {
        var existingUser = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Idusuario == id);
        if (existingUser == null)
        {
            return false;
        }

        // Validación de unicidad para NombreUsuario en actualización
        var userWithSameUsername = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.NombreUsuario == usuarioDto.NombreUsuario && u.Idusuario != id);
        if (userWithSameUsername != null)
        {
            throw new InvalidOperationException($"El nombre de usuario '{usuarioDto.NombreUsuario}' ya está en uso por otro usuario.");
        }

        var usuarioToUpdate = new Usuario
        {
            Idusuario = id,
            Nombre = usuarioDto.Nombre,
            NombreUsuario = usuarioDto.NombreUsuario,
            Idperfil = usuarioDto.Idperfil,
            Estatus = usuarioDto.Estatus,
            Fechacreacion = existingUser.Fechacreacion
        };

        if (!string.IsNullOrEmpty(usuarioDto.Password) && !BCrypt.Net.BCrypt.Verify(usuarioDto.Password, existingUser.Password))
        {
            usuarioToUpdate.Password = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Password);
        }
        else
        {
            usuarioToUpdate.Password = existingUser.Password;
        }

        _context.Entry(usuarioToUpdate).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UsuarioExists(id))
            {
                return false;
            }
            else
            {
                throw;
            }
        }
    }

    public async Task<bool> DeleteUsuarioAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return false;
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return true;
    }

    private bool UsuarioExists(int id)
    {
        return (_context.Usuarios?.Any(e => e.Idusuario == id)).GetValueOrDefault();
    }
}