using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatABook.Models;

public class Orden
{
    [Key]
    public int OrdenId { get; set; }

    [ForeignKey("ApplicationUser")]
    public string? ClienteId { get; set; }

    public string? Nombre { get; set; }

    public string? Telefono { get; set; }

    public DateTime Fecha { get; set; }

    [ForeignKey("MetodosDePagos")]
    public int MetodoDePagoId { get; set; }

    public double Monto { get; set; }

    public ICollection<OrdenDetalle> OrdenDetalle { get; set; } = new List<OrdenDetalle>();

    //public MetodosDePagos? MetodoDePagos { get; set; }
}