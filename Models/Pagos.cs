using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WhatABook.Models
{
	public class Pagos
	{
		[Key]
		public int PagoId { get; set; }

		[Required]
		[ForeignKey("Clientes")]
		public int ClienteId { get; set; }
		public Clientes Clientes { get; set; }

		public string MetodoPagoId { get; set; }

		[Required]
		[Range(0.01, double.MaxValue, ErrorMessage = "El valor no puedo ser menor o igual a 0.")]
		public decimal MontoTotal { get; set; }
		
		[Required]
		public DateTime FechaPago { get; set; } = DateTime.Now;
	}
}
