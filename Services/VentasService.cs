using Microsoft.EntityFrameworkCore;
using WhatABook.DAL;
using WhatABook.Models;

namespace WhatABook.Services
{
	public class VentasService(IDbContextFactory<Contexto> DbFactory)
	{
		public async Task<bool> Guardar(Ventas venta, List<VentasDetalles> detallesVenta)
		{
			if (await Existe(venta.VentaId))
				return await Modificar(venta, detallesVenta);
			else
				return await Insertar(venta, detallesVenta);
		}

		private async Task<bool> Existe(int ventaId)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			return await contexto.Ventas.AnyAsync(v => v.VentaId == ventaId);
		}

		private async Task<bool> Insertar(Ventas venta, List<VentasDetalles> detallesVenta)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			contexto.Ventas.Add(venta);

			foreach (var detalle in detallesVenta)
			{
				var libro = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroId == detalle.LibroId);

				if (libro != null)
				{
					libro.Cantidad -= detalle.Cantidad;  // Restar cantidad al inventario
					contexto.VentasDetalles.Add(detalle);
				}
			}

			var result = await contexto.SaveChangesAsync();
			return result > 0;
		}

		private async Task<bool> Modificar(Ventas venta, List<VentasDetalles> detallesVenta)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			var ventaExistente = await contexto.Ventas.Include(v => v.Clientes).FirstOrDefaultAsync(v => v.VentaId == venta.VentaId);

			if (ventaExistente == null)
				return false;

			// Obtener detalles antiguos para revertir inventario
			var detallesAntiguos = await contexto.VentasDetalles.Where(d => d.VentaId == venta.VentaId).ToListAsync();

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
					contexto.VentasDetalles.Update(detalle);
				}
			}

			// Actualizar datos de la venta
			ventaExistente.ClienteId = venta.ClienteId;
			ventaExistente.FechaVenta = venta.FechaVenta;
			ventaExistente.PagoId = venta.PagoId;

			var result = await contexto.SaveChangesAsync();
			return result > 0;
		}

		public async Task<bool> Eliminar(int ventaId)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			var venta = await contexto.Ventas.Include(v => v.Clientes).FirstOrDefaultAsync(v => v.VentaId == ventaId);

			if (venta == null)
				return false;

			var detalles = await contexto.VentasDetalles.Where(d => d.VentaId == ventaId).ToListAsync();

			foreach (var detalle in detalles)
			{
				var libro = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroId == detalle.LibroId);

				if (libro != null)
				{
					libro.Cantidad += detalle.Cantidad;  // Revertir cantidad al inventario
				}
			}

			contexto.Ventas.Remove(venta);
			return await contexto.SaveChangesAsync() > 0;
		}

		public async Task<List<Ventas>> ObtenerHistorialCompras(int clienteId)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			return await contexto.Ventas
				.Include(v => v.Clientes)
				.Include(v => v.VentaDetalles)
				.ThenInclude(d => d.Libros)
				.Where(v => v.ClienteId == clienteId)
				.ToListAsync();
		}

		public async Task<List<Ventas>> ObtenerHistorialVentas()
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			return await contexto.Ventas
				.Include(v => v.Clientes)
				.Include(v => v.VentaDetalles)
				.ThenInclude(d => d.Libros)
				.ToListAsync();
		}
	}
}
