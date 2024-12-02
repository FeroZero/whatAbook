using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatABook.Models;

public class Ventas
{
	[Key]
	public int VentaId { get; set; }

	[Required]
	[ForeignKey("Clientes")]
	public int ClienteId { get; set; }
	public Clientes Clientes { get; set; }

	[Required]
	[ForeignKey("Pagos")]
	public int PagoId { get; set; }
	public Pagos Pagos { get; set; }

	[Required(ErrorMessage = "Fecha Requerida")]
	public DateTime FechaVenta { get; set; } = DateTime.Now;

	public virtual ICollection<VentasDetalles> VentaDetalles { get; set; } = new List<VentasDetalles>();
}