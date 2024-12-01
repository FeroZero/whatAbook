using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WhatABook.Data;
using WhatABook.Models;

namespace WhatABook.Services;

public class LibrosService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    

    public async Task<bool> Guardar(Libros libro)
    {
        if (!await Existe(libro.LibroId))
            return await Insertar(libro);
        else
            return await Modificar(libro);
    }

    public async Task<bool> Insertar(Libros libro)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Libros.Add(libro);
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Existe(int libroId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Libros
            .AnyAsync(l => l.LibroId == libroId);
    }

    private async Task<bool> Modificar(Libros libro)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Update(libro);
        var modificado = await contexto.SaveChangesAsync() > 0;
        contexto.Entry(libro).State = EntityState.Modified;
        return modificado;

    }

    public async Task<bool> Eliminar(int libroId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Libros
            .AsNoTracking()
            .Where(l => l.LibroId == libroId)
            .ExecuteDeleteAsync() > 0;
    }

    public async Task<Libros?> Buscar(int libroId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Libros
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.LibroId == libroId);
    }

    public async Task<List<Libros>> Listar(Expression<Func<Libros, bool>> criterio)
    {

        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Libros
            .AsNoTracking()
            .Where(criterio)
            .ToListAsync();

    }



}
