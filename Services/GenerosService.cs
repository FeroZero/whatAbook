using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Linq.Expressions;
using WhatABook.DAL;
using WhatABook.Models;

namespace WhatABook.Services;

public class GenerosService(IDbContextFactory<Contexto> DbFactory)
{
	public async Task<bool> Guardar(Generos genero)
	{
		if (await Existe(genero.GeneroId))
			return await Insertar(genero);
		else
			return await Modficar(genero);
	}

	private async Task<bool> Modficar(Generos genero)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		contexto.Update(genero);
		return await contexto.SaveChangesAsync() > 0;
	}

	private async Task<bool> Insertar(Generos genero)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		contexto.Generos.Add(genero);
		return await contexto.SaveChangesAsync() > 0;
	}

	private async Task<bool> Existe(int Id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Generos.AnyAsync(g => g.GeneroId == Id);
	}

	public async Task<bool> Eliminar(int Id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Generos
			.Where(e => e.GeneroId == Id)
			.ExecuteDeleteAsync() > 0;
	}

	public async Task<Generos?> Buscar(int Id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Generos
			.AsNoTracking()
			.FirstOrDefaultAsync(c => c.GeneroId == Id);
	}

	public async Task<List<Generos>> Listar(Expression<Func<Generos, bool>> criterio)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Generos
			.AsNoTracking()
			.Where(criterio)
			.ToListAsync();
	}
}
