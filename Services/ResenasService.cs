using Microsoft.EntityFrameworkCore;
using WhatABook.Data;
using WhatABook.Models;

namespace WhatABook.Services;

public class ResenasService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Guardar(Resenas resena)
    {
        if (await Existe(resena.ResenaId))
            return await Modificar(resena);
        else
            return await Insertar(resena);
    }

    private async Task<bool> Existe(int resenaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Resenas.AnyAsync(r => r.ResenaId == resenaId);
    }

    private async Task<bool> Insertar(Resenas resena)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Resenas.Add(resena);
        var result = await contexto.SaveChangesAsync();
        return result > 0;
    }

    private async Task<bool> Modificar(Resenas resena)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Resenas.Update(resena);
        var result = await contexto.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> Eliminar(int resenaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var resena = await contexto.Resenas.FindAsync(resenaId);
        if (resena == null)
            return false;

        contexto.Resenas.Remove(resena);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<List<Resenas>> ListarResenas()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Resenas.Include(r => r.Libros).ToListAsync();
    }

    public async Task<List<Resenas>> BuscarResenas(string keyword)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Resenas
            .Include(r => r.Libros)
            .Where(r => r.Comentario.Contains(keyword))
            .ToListAsync();
    }
}
