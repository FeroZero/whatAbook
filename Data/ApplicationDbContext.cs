using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WhatABook.Models;

namespace WhatABook.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
    public DbSet<Libros> Libros { get; set; }
    public DbSet<Generos> Generos { get; set; }
    public DbSet<Resenas> Resenas { get; set; }

    public DbSet<Orden> Orden { get; set; }
    public DbSet<OrdenDetalle> OrdenDetalle { get; set; }

    public DbSet<Ventas> Ventas { get; set; }
    public DbSet<VentasDetalles> VentasDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        ConfigureGeneralModel(builder);
    }

    public void ConfigureGeneralModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MetodosDePagos>().HasData(
            new MetodosDePagos { MetodoDePagoId = 1, Tipo = "Credito" },
            new MetodosDePagos { MetodoDePagoId = 2, Tipo = "Debito" }
        );
    }
}

