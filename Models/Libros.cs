using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatABook.Models;

public class Libros
{
    [Key]
    public int LibroId { get; set; }

    [Required(ErrorMessage = " Campo obligatorio")]
    public string? Titulo { get; set; }

    [Required(ErrorMessage = " Campo obligatorio")]
    [RegularExpression(@"[A-Za-z\s]+$", ErrorMessage = "No se permiten caracteres especiales o numeros")]
    public string? Autores { get; set; }

    [Required(ErrorMessage = " Campo obligatorio")]

    [ForeignKey("Generos")]
    public string GeneroId { get; set; }
    public Generos Genero { get; set; }

    [Required(ErrorMessage = " Campo obligatorio")]
    [RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "No se permiten letras.")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = " Campo obligatorio")]
    public DateOnly FechaPublicacion { get; set; }

    [Required(ErrorMessage = " Campo obligatorio")]
    public string? Estado { get; set; }

    [Required(ErrorMessage = " Campo obligatorio")]
    
    public string? Descripcion { get; set; }

	[Required(ErrorMessage = " Campo obligatorio")]
    [Range(0,int.MaxValue)]
	public int Cantidad { get; set; }

    [Required(ErrorMessage = " Campo obligatorio")]
    public  string? ImagenUrl { get; set; }
}   
