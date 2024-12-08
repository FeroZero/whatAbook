using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatABook.Models;

public class OrdenDetalle
{
    [Key]
    public int OrdenDetalleId { get; set; }

    [ForeignKey("OrdenId")]
    public int OrdenId { get; set; }

    [ForeignKey("LibroId")]
    public int LibroId { get; set; }

    [Required(ErrorMessage = "Campo Obligatorio")]
    [Range(0.01, 10000000, ErrorMessage = "El precio debe estar entre 0.01 y 1000000")]
    public double Precio { get; set; }

    [Required(ErrorMessage = "La cantidad es requerida")]
    public int Cantidad{ get; set; }

    public Libros? Libros { get; set; }
    public Orden? Orden { get; set; }

}
