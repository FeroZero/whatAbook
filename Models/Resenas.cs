using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WhatABook.Data;

namespace WhatABook.Models
{
    public class Resenas
    {
        [Key]
        public int ResenaId { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public string? UsuarioId { get; set; }

        [Required]
        [ForeignKey("Libro")]
        public int LibroId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
        public int Calificación { get; set; }

        [Required(ErrorMessage = "El comentario es obligatorio.")]
        [StringLength(1000, ErrorMessage = "El comentario no puede tener más de 1000 caracteres.")]
        public string Comentario { get; set; }

        public virtual ApplicationUser Usuario { get; set; }
        public virtual Libros Libros { get; set; }
    }
}
