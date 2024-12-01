using Microsoft.EntityFrameworkCore;
using WhatABook.DAL;
using WhatABook.Models;

namespace WhatABook.Services
{
	public class CarritoDeComprasService(IDbContextFactory<Contexto> DbFactory)
	{
		public async Task<bool> GuardarCarritoDeCompra(Carritos carrito)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			if (await contexto.Carritos.AnyAsync(c => c.CarritoId == carrito.CarritoId))
			{
				contexto.Carritos.Update(carrito);
			}
			else
			{
				contexto.Carritos.Add(carrito);
			}

			return await contexto.SaveChangesAsync() > 0;
		}

		// Agregar un libro al carrito
		public async Task<bool> AgregarLibroAlCarrito(Carritos carrito, CarritoDetalles detalle)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();

			var carritoExistente = await ObtenerCarritoDeCompra(carrito.ClienteId);
			if (carritoExistente == null)
			{
				contexto.Carritos.Add(carrito);
				await contexto.SaveChangesAsync();
				carritoExistente = carrito;
			}

			var detalleExistente = await contexto.CarritoDetalles
				.FirstOrDefaultAsync(d => d.CarritoId == carritoExistente.CarritoId && d.LibroId == detalle.LibroId);
			if (detalleExistente != null)
			{
				detalleExistente.Cantidad += detalle.Cantidad;
				contexto.CarritoDetalles.Update(detalleExistente);
			}
			else
			{
				detalle.CarritoId = carritoExistente.CarritoId;
				contexto.CarritoDetalles.Add(detalle);
			}

			return await contexto.SaveChangesAsync() > 0;
		}

		// Obtener el carrito de compra de un cliente
		public async Task<Carritos> ObtenerCarritoDeCompra(int clienteId)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			return await contexto.Carritos
				.Include(c => c.CarritoDetalles)
				.ThenInclude(d => d.Libros)
				.FirstOrDefaultAsync(c => c.ClienteId == clienteId);
		}

		// Eliminar un detalle del carrito
		public async Task<bool> EliminarDetalle(CarritoDetalles detalle)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			var detalleExistente = await contexto.CarritoDetalles
				.FirstOrDefaultAsync(d => d.CarritoDetalleId == detalle.CarritoDetalleId);
			if (detalleExistente == null) return false;

			contexto.CarritoDetalles.Remove(detalleExistente);
			return await contexto.SaveChangesAsync() > 0;
		}

		// Vaciar el carrito de un cliente
		public async Task<bool> VaciarCarrito(Carritos carrito)
		{
			await using var contexto = await DbFactory.CreateDbContextAsync();
			var carritoExistente = await ObtenerCarritoDeCompra(carrito.ClienteId);
			if (carritoExistente == null) return false;

			contexto.CarritoDetalles.RemoveRange(carritoExistente.CarritoDetalles);
			return await contexto.SaveChangesAsync() > 0;
		}
	}
}
