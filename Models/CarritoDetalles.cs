using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WhatABook.Models;

public class CarritoDetalles
{
	[Key]
	public int CarritoDetalleId { get; set; }

	[ForeignKey("CarritoDeCompra")]
	public int CarritoId { get; set; }
	public Carritos CarritoDeCompra { get; set; }

	[ForeignKey("Libro")]
	public int LibroId { get; set; }
	public Libros Libros { get; set; }

	[Required(ErrorMessage = "La cantidad es obligatoria.")]
	[Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
	public int Cantidad { get; set; }
}
