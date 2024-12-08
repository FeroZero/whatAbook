using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WhatABook.Models;

public class VentasDetalles
{
	[Key]
	public int VentaDetalleId { get; set; }

	[ForeignKey("Ventas")]
	public int VentaId { get; set; }
	public Ventas? Ventas { get; set; }

	[ForeignKey("Libros")]
	public int LibroId { get; set; }
    public Libros? Libros { get; set; }

    [Required]
	[Range(0,int.MaxValue, ErrorMessage = "La cantidad no puede ser menor a 0.")]
	public int Cantidad { get; set; }

	[Required(ErrorMessage = "Precio Requerido.")]
	[Range(0.01, double.MaxValue, ErrorMessage = "El valor no puedo ser menor o igual a 0.")]
	public double Precio { get; set; }
}
