using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatABook.Models;

public class Ventas
{
	[Key]
	public int VentaId { get; set; }

	[Required]
	[ForeignKey("ApplicationUser")]
	public int ClienteId { get; set; }

	[ForeignKey("OrdenId")]
    public int OrdenId { get; set; }

	[ForeignKey("MetodoDePagoId")]
    public int MetodoDePagoId { get; set; }

    public DateTime FechaVenta { get; set; } = DateTime.Now;

	public virtual ICollection<VentasDetalles> VentaDetalles { get; set; } = new List<VentasDetalles>();
}