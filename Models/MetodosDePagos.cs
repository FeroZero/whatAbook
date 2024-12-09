using System.ComponentModel.DataAnnotations;

namespace WhatABook.Models;

public class MetodosDePagos
{
    [Key]
    public int MetodoDePagoId { get; set; }

    [Required(ErrorMessage = "Campo Obligatorio")]
    public string? Tipo { get; set; }

    public Orden? Orden { get; set; }
}
