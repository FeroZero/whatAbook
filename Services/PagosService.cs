using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WhatABook.Data;
using WhatABook.Models;

namespace WhatABook.Services
{
	public class PagosService(IDbContextFactory<ApplicationDbContext> DbFactory)
	{
		public async Task<bool> Guardar(Pagos pagos, List<VentasDetalles> detallesVenta)
		{
			if (await Existe(pagos.PagoId))
				return await Insertar(pagos, detallesVenta);
			else
				return await Modificar(pagos, detallesVenta);
		}

		private async Task<bool> Modificar(Pagos pagos, List<VentasDetalles> detallesVenta)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			var pagoExistente = await contexto.Pagos.Include(p => p.Clientes).FirstOrDefaultAsync(p => p.PagoId == pagos.PagoId);

			if (pagoExistente == null)
				return false;

			// Obtener detalles antiguos para revertir inventario
			var detallesAntiguos = await contexto.VentasDetalles.Where(d => d.VentaId == pagos.PagoId).ToListAsync();

			foreach (var detalle in detallesAntiguos)
			{
				var libro = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroId == detalle.LibroId);

				if (libro != null)
				{
					libro.Cantidad += detalle.Cantidad;  // Revertir cantidad al inventario
				}
			}

			// Actualizar detalles nuevos y ajustar inventario
			foreach (var detalle in detallesVenta)
			{
				var libro = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroId == detalle.LibroId);

				if (libro != null)
				{
					libro.Cantidad -= detalle.Cantidad;  // Restar cantidad al inventario
				}
			}

			// Actualizar datos del pago
			pagoExistente.MontoTotal = pagos.MontoTotal;
			pagoExistente.MetodoPagoId = pagos.MetodoPagoId;
			pagoExistente.FechaPago = pagos.FechaPago;

			var result = await contexto.SaveChangesAsync();
			return result > 0;
		}

		private async Task<bool> Insertar(Pagos pago, List<VentasDetalles> detallesVenta)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			contexto.Pagos.Add(pago);

			foreach (var detalle in detallesVenta)
			{
				var libro = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroId == detalle.LibroId);

				if (libro != null)
				{
					libro.Cantidad -= detalle.Cantidad;  // Restar cantidad al inventario
				}
			}

			var result = await contexto.SaveChangesAsync();
			return result > 0;
		}

		private async Task<bool> Existe(int Id)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			return await contexto.Pagos.AnyAsync(p => p.PagoId == Id);
		}

		public async Task<bool> Eliminar(int pagoId)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			var pago = await contexto.Pagos.Include(p => p.Clientes).FirstOrDefaultAsync(p => p.PagoId == pagoId);

			if (pago == null)
				return false;

			var detalles = await contexto.VentasDetalles.Where(d => d.VentaId == pagoId).ToListAsync();

			foreach (var detalle in detalles)
			{
				var libro = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroId == detalle.LibroId);

				if (libro != null)
				{
					libro.Cantidad += detalle.Cantidad;  // Revertir cantidad al inventario
				}
			}

			contexto.Pagos.Remove(pago);
			return await contexto.SaveChangesAsync() > 0;
		}

		public async Task<Pagos?> Buscar(int Id)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			return await contexto.Pagos
				.AsNoTracking()
				.FirstOrDefaultAsync(c => c.PagoId == Id);
		}

		public async Task<List<Pagos>> Listar(Expression<Func<Pagos, bool>> criterio)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			return await contexto.Pagos
				.AsNoTracking()
				.Where(criterio)
				.ToListAsync();
		}
	}
}
