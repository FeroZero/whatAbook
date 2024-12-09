using Microsoft.EntityFrameworkCore;
using WhatABook.Data;
using WhatABook.Models;

namespace WhatABook.Services
{
    public class OrdenService(IDbContextFactory<ApplicationDbContext> DbFactory)
    {

        public async Task<bool> Guardar(Orden orden, List<OrdenDetalle> detallesOrden)
        {
            if (await Existe(orden.OrdenId))
                return await Modificar(orden, detallesOrden);
            else
                return await Insertar(orden, detallesOrden);
        }

        private async Task<bool> Existe(int ordenId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Orden.AnyAsync(o => o.OrdenId == ordenId);
        }

        private async Task<bool> Insertar(Orden orden, List<OrdenDetalle> detallesOrden)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.Orden.Add(orden);

            foreach (var detalle in detallesOrden)
            {
                var libro = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroId == detalle.LibroId);

                if (libro != null)
                {
                    libro.Cantidad -= detalle.Cantidad;  // Restar cantidad al inventario
                    contexto.OrdenDetalle.Add(detalle);
                }
            }

            var result = await contexto.SaveChangesAsync();
            return result > 0;
        }

        private async Task<bool> Modificar(Orden orden, List<OrdenDetalle> detallesOrden)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var ordenExistente = await contexto.Orden.Include(o => o.OrdenDetalle).FirstOrDefaultAsync(o => o.OrdenId == orden.OrdenId);

            if (ordenExistente == null)
                return false;

            // Obtener detalles antiguos para revertir inventario
            var detallesAntiguos = await contexto.OrdenDetalle.Where(d => d.OrdenId == orden.OrdenId).ToListAsync();

            foreach (var detalle in detallesAntiguos)
            {
                var libro = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroId == detalle.LibroId);

                if (libro != null)
                {
                    libro.Cantidad += detalle.Cantidad;  // Revertir cantidad al inventario
                }
            }

            // Actualizar detalles nuevos y ajustar inventario
            foreach (var detalle in detallesOrden)
            {
                var libro = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroId == detalle.LibroId);

                if (libro != null)
                {
                    libro.Cantidad -= detalle.Cantidad;  // Restar cantidad al inventario
                    contexto.OrdenDetalle.Update(detalle);
                }
            }

            // Actualizar datos de la orden
            ordenExistente.Fecha = orden.Fecha;
            ordenExistente.MetodoDePagoId = orden.MetodoDePagoId;
            ordenExistente.ClienteId = orden.ClienteId;
            ordenExistente.Nombre = orden.Nombre;
            ordenExistente.Telefono = orden.Telefono;
            ordenExistente.Monto = orden.Monto;

            var result = await contexto.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> Eliminar(int ordenId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var orden = await contexto.Orden.Include(o => o.OrdenDetalle).FirstOrDefaultAsync(o => o.OrdenId == ordenId);

            if (orden == null)
                return false;

            var detalles = await contexto.OrdenDetalle.Where(d => d.OrdenId == ordenId).ToListAsync();

            foreach (var detalle in detalles)
            {
                var libro = await contexto.Libros.FirstOrDefaultAsync(l => l.LibroId == detalle.LibroId);

                if (libro != null)
                {
                    libro.Cantidad += detalle.Cantidad;  // Revertir cantidad al inventario
                }
            }

            contexto.Orden.Remove(orden);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<List<Orden>> ObtenerHistorialOrdenes()
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.Orden
                .Include(o => o.OrdenDetalle)
                .ThenInclude(d => d.Libros)
                .ToListAsync();
        }
    }

}
