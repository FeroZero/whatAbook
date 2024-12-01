using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WhatABook.Models;

public class Carritos
{
	[Key]
	public int CarritoId { get; set; }

	[ForeignKey("Clientes")]
	public int ClienteId { get; set; }
	public Clientes Clientes { get; set; }

	public virtual ICollection<CarritoDetalles> CarritoDetalles { get; set; } = new List<CarritoDetalles>();
}
