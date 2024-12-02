using System.ComponentModel.DataAnnotations;

namespace WhatABook.Models;

public class Generos
{
    [Key]
    public int GeneroId { get; set; }

    [Required(ErrorMessage = " Campo obligatorio")]
    public string? TipoGeneros { get; set; }

    public virtual ICollection<Libros> Libros { get; set; } = new List<Libros>();
}
