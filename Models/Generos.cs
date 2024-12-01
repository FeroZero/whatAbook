using System.ComponentModel.DataAnnotations;

namespace WhatABook.Models;

public class Generos
{
    [Key]
    public int GeneroId { get; set; }

    [Required(ErrorMessage = " Campo obligatorio")]
    public string? Nombres { get; set; }

    [Required(ErrorMessage = " Campo obligatorio")]
    public string? Descripcion { get; set; }

    public virtual ICollection<Libros> Libros { get; set; } = new List<Libros>();
}
