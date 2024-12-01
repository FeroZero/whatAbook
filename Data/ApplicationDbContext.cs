using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WhatABook.Models;

namespace WhatABook.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
	public DbSet<Clientes> Clientes { get; set; }
	public DbSet<Libros> Libros { get; set; }
	public DbSet<Generos> Generos { get; set; }
	public DbSet<Pagos> Pagos { get; set; }
	public DbSet<Ventas> Ventas { get; set; }
	public DbSet<VentasDetalles> VentasDetalles { get; set; }
	public DbSet<Carritos> Carritos { get; set; }
	public DbSet<CarritoDetalles> CarritoDetalles { get; set; }
}
