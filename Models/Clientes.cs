using System.ComponentModel.DataAnnotations;

namespace WhatABook.Models;

public class Clientes
{
	[Key]
	public int ClienteId { get; set; }

	[Required(ErrorMessage = "Debe digitar un numero de telefono.")]
	[RegularExpression(@"^[\d{10}]+$", ErrorMessage = "Numero de telefono.")]
	public string Telefono { get; set; }

	[Required(ErrorMessage = "Debe poner un Nombre")]
	[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Solo letras.")]
	public string Nombre { get; set; }

	[Required(ErrorMessage = "Se requiere un correo electronico.")]
	[RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Correo electrónico no válido.")]
	public string Correo { get; set; }
	public DateTime FechaRegistro { get; set; } = DateTime.Now;

	public virtual ICollection<Ventas> Ventas { get; set; } = new List<Ventas>();
	public virtual ICollection<Pagos> Pagos { get; set; } = new List<Pagos>();
}

