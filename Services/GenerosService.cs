using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Linq.Expressions;
using WhatABook.Data;
using WhatABook.Models;

namespace WhatABook.Services;

public class GenerosService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
	public async Task<bool> Guardar(Generos genero)
	{
		if (await Existe(genero.GeneroId))
			return await Insertar(genero);
		else
			return await Modficar(genero);
	}

	private async Task<bool> Modficar(Generos genero)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		contexto.Update(genero);
		return await contexto.SaveChangesAsync() > 0;
	}

	private async Task<bool> Insertar(Generos genero)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		contexto.Generos.Add(genero);
		return await contexto.SaveChangesAsync() > 0;
	}

	private async Task<bool> Existe(int Id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Generos.AnyAsync(g => g.GeneroId == Id);
	}

	public async Task<bool> Eliminar(int Id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Generos
			.Where(e => e.GeneroId == Id)
			.ExecuteDeleteAsync() > 0;
	}

	public async Task<Generos?> Buscar(int Id)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Generos
			.AsNoTracking()
			.FirstOrDefaultAsync(c => c.GeneroId == Id);
	}

    public List<Generos> ObtenerGeneros()
    {
        // Aquí deberías retornar los géneros con sus libros predefinidos.
        return Generos;
    }


    private List<Generos> Generos = new List<Generos>
        {
            new Generos
            {
                TipoGeneros = "Fantasía",
                Libros = new List<Libros>
                {
                    new Libros { Titulo = "Harry Potter", Autores = "J.K. Rowling", Precio = 19.99, ImagenUrl = "https://pekeleke.es/wp-content/uploads/2020/11/harry-potter-piedra-filosofal-minalima-portada.jpg" },
                    new Libros { Titulo = "The Hobbit", Autores = "J.R.R. Tolkien", Precio = 15.99, ImagenUrl = "https://th.bing.com/th/id/OIP.jPKl0ooFbgYAhrboBbkE9AHaLg?rs=1&pid=ImgDetMain" },
                    new Libros { Titulo = "Juego De Tronos", Autores = "George R.R. Martin", Precio = 24.99, ImagenUrl = "https://th.bing.com/th/id/R.0b4221eb723fcb6c0c674461db0efa16?rik=ft1JAY3eC9ZRkA&riu=http%3a%2f%2f4.bp.blogspot.com%2f-RfUNZ7b0q3k%2fUCLTqtxK3GI%2fAAAAAAAAAPE%2f9XWUkIdHWNg%2fs1600%2fjuego%2bde%2btronos.jpg&ehk=GpWlqEfaUjZ4grLVswNto8z9LTHB7HFD1CSGKHZJykM%3d&risl=&pid=ImgRaw&r=0" },
                    new Libros { Titulo = "La Rueda Del Tiempo", Autores = "Robert Jordan", Precio = 22.50, ImagenUrl = "https://th.bing.com/th/id/R.4fc6911504394a9e7837225606158df0?rik=9iphpMyt0woGZQ&pid=ImgRaw&r=0" },
                    new Libros { Titulo = "El Señor De Los Anillos", Autores = "J.R.R. Tolkien", Precio = 29.99, ImagenUrl = "https://th.bing.com/th/id/OIP.XnBjus1SHsYwZeuRtsC5NgHaLZ?rs=1&pid=ImgDetMain" },
                    new Libros { Titulo = "Moby Dick", Autores = "Herman Melville", Precio = 15.99, ImagenUrl = "https://th.bing.com/th/id/R.ae0744397ef02d50a9608b8f8ae75c04?rik=QWx0lWzWLw9mjg&pid=ImgRaw&r=0" },
                }
            },
            new Generos
            {
                TipoGeneros = "Romance",
                Libros = new List<Libros>
                {
                    new Libros { Titulo = "Orgullo y Prejuicio", Autores = "Jane Austen", Precio = 12.99, ImagenUrl = "https://imagessl2.casadellibro.com/a/l/t0/82/9788415618782.jpg" },
                    new Libros { Titulo = "El Cuaderno de Noah", Autores = "Nicholas Sparks", Precio = 18.50, ImagenUrl = "https://cdn.agapea.com//Roca-Bolsillo/El-cuaderno-de-Noah-i6n17397244.jpg" },
                    new Libros { Titulo = "Bajo La Misma Estrella", Autores = "John Green", Precio = 14.99, ImagenUrl = "https://th.bing.com/th/id/OIP.mcOb4qfvil1pVTB1KDGG6wHaLQ?rs=1&pid=ImgDetMain" },
                    new Libros { Titulo = "Jane Eyre", Autores = "Charlotte Brontë", Precio = 17.99, ImagenUrl = "https://th.bing.com/th/id/OIP.HxY7KwxNhXzvkQfYRA1ypgHaLW?rs=1&pid=ImgDetMain" },
                    new Libros { Titulo = "Yo Antes De Ti", Autores = "Jojo Moyes", Precio = 16.50, ImagenUrl = "https://th.bing.com/th/id/R.bb3207b79739d59e18e9402f326d526b?rik=ZKbwFgIay6HFkQ&pid=ImgRaw&r=0" },
                    new Libros { Titulo = "Eleanor y Park", Autores = "Rainbow Rowell", Precio = 13.99, ImagenUrl = "https://th.bing.com/th/id/OIP.lUANBwGuLKqnXAxth2HSFwHaLN?rs=1&pid=ImgDetMain" }
                }
            },
            new Generos
            {
                TipoGeneros = "Terror",
                Libros = new List<Libros>
                {

                    new Libros { Titulo = "Cementerio De Animales", Autores = "Stephen King", Precio = 18.99, ImagenUrl = "https://th.bing.com/th/id/OIP.4JW3kH3clatCREQE-MiZ-AHaLb?rs=1&pid=ImgDetMain" },
                    new Libros { Titulo = "El Exorcista", Autores = "William Peter Blatty", Precio = 19.50, ImagenUrl = "https://th.bing.com/th/id/R.5f2f527bd7ded7c0d1f33a66666351a0?rik=m6KW86yuleyIbg&pid=ImgRaw&r=0" },
                    new Libros { Titulo = "Dracula", Autores = "Bram Stoker", Precio = 15.99, ImagenUrl = "https://th.bing.com/th/id/OIP.tLgDw3d3WSwNuX9jYqNQxwHaKG?rs=1&pid=ImgDetMain" },
                    new Libros { Titulo = "La Casa Infernal", Autores = "Richard Matheson", Precio = 20.99, ImagenUrl = "https://th.bing.com/th/id/OIP.Vm_Uj2bROdCGnmhGUh0oRQHaLb?rs=1&pid=ImgDetMain" },
                    new Libros { Titulo = "Frankenstein", Autores = "Mary Shelley", Precio = 14.50, ImagenUrl = "https://th.bing.com/th/id/OIP.rI9REi9gAubz7uKIyQ6FcQHaLH?rs=1&pid=ImgDetMain" },
                    new Libros { Titulo = "El Resplandor", Autores = "Stephen King", Precio = 21.50, Descripcion = "Una novela de terror psicológico que explora la locura y el mal sobrenatural en el Hotel Overlook.", ImagenUrl = "https://th.bing.com/th/id/R.42b7ab9efbdf90649d9c7ed71cc89404?rik=uRhQgKC1YZU6Mg&pid=ImgRaw&r=0" },
                }
            },

            new Generos
            {
                TipoGeneros = "Ciencia Ficcion",
                Libros = new List<Libros>
                {
                    new Libros { Titulo = "Dune", Autores = "Frank Herbert", Precio = 24.99, Descripcion = "Una épica historia de ciencia ficción que sigue a Paul Atreides en un planeta desértico lleno de intriga política y guerra.", ImagenUrl = "https://th.bing.com/th/id/R.9e72371e420c20bc6cbcda90a42d97d6?rik=u7MeK5iE4iaNKw&pid=ImgRaw&r=0" },
                    new Libros { Titulo = "Neuromante", Autores = "William Gibson", Precio = 19.99, Descripcion = "Una novela pionera del ciberpunk que introduce a los lectores al mundo de la inteligencia artificial y los hackers.", ImagenUrl = "https://th.bing.com/th/id/R.25712f0021e5fa3520661555bcaa1c3e?rik=fYmPLmaA3HXp4g&pid=ImgRaw&r=0" },
                    new Libros { Titulo = "1984", Autores = "George Orwell", Precio = 15.50, Descripcion = "Un clásico distópico que explora un mundo controlado por un régimen totalitario y la vigilancia extrema.", ImagenUrl = "https://th.bing.com/th/id/OIP.8bTpGDLEYVpDlV8tD10XlQAAAA?rs=1&pid=ImgDetMain" },
                    new Libros { Titulo = "El Juego de Ender", Autores = "Orson Scott Card", Precio = 18.99, Descripcion = "Una fascinante historia de ciencia ficción donde un joven prodigio debe salvar a la humanidad de una amenaza alienígena.", ImagenUrl = "https://th.bing.com/th/id/R.f9416b31769365587404e451a2d69410?rik=YFw6NiB0v5872Q&riu=http%3a%2f%2fwww.librosyliteratura.es%2fwp-content%2fuploads%2f2017%2f05%2fel-juego-de-ender.jpg&ehk=UBu6yTWZ1gBf6YNeNVPA%2fba0pwEyhCIl8cgZDXzseE4%3d&risl=&pid=ImgRaw&r=0" },
                    new Libros { Titulo = "La Guerra de los Mundos", Autores = "H.G. Wells", Precio = 14.99, Descripcion = "Un relato de invasión alienígena que revolucionó la ciencia ficción.", ImagenUrl = "https://th.bing.com/th/id/OIP.AVA4BAL083IokW_NjQe3EwHaLW?rs=1&pid=ImgDetMain" },
                    new Libros { Titulo = "Fahrenheit 451", Autores = "Ray Bradbury", Precio = 16.50, Descripcion = "Un clásico que explora un futuro donde los libros están prohibidos y los bomberos los queman.", ImagenUrl = "https://th.bing.com/th/id/R.3e76c2fc5a3f9ea5afbdd34ba1ec2e90?rik=%2fnL43hQ9hH70SA&pid=ImgRaw&r=0" },

                }
            },

            new Generos
            {
                TipoGeneros = "Clasicos",
                Libros = new List<Libros>
                {
                    new Libros { Titulo = "Matar A Un Ruiseñor", Autores = "Harper Lee", Precio = 14.50, Descripcion = "Una poderosa historia sobre la injusticia racial en el sur de Estados Unidos vista a través de los ojos de una niña.", ImagenUrl = "https://http2.mlstatic.com/D_NQ_NP_992079-MLM47037519198_082021-F.jpg" },
                    new Libros { Titulo = "Cien Años de Soledad", Autores = "Gabriel García Márquez", Precio = 20.99, Descripcion = "Una obra maestra del realismo mágico que narra la historia de la familia Buendía en el pueblo ficticio de Macondo.", ImagenUrl = "https://th.bing.com/th/id/R.95e74b9ee1dc63d99f7733da2794dd33?rik=p8GxjyH6%2f1qj7Q&pid=ImgRaw&r=0" },
                    new Libros { Titulo = "Crimen y Castigo", Autores = "Fiódor Dostoyevski", Precio = 18.50, Descripcion = "Una intensa exploración de la moralidad y la psicología de un hombre que comete un asesinato.", ImagenUrl = "https://th.bing.com/th/id/R.aed382fd7f8f407a700e14fb7af4494e?rik=rJtwFZAOOWdDnw&pid=ImgRaw&r=0" },
                    new Libros { Titulo = "Don Quijote de la Mancha", Autores = "Miguel de Cervantes", Precio = 22.50, Descripcion = "La icónica novela que relata las aventuras de un caballero idealista y su fiel escudero.", ImagenUrl = "https://th.bing.com/th/id/OIP.p2Qht0iMCaSgQb3wbzHukAHaKN?rs=1&pid=ImgDetMain" },
                    new Libros { Titulo = "Grandes Esperanzas", Autores = "Charles Dickens", Precio = 17.99, Descripcion = "La historia de un joven huérfano, Pip, y su camino hacia el autodescubrimiento y la madurez.", ImagenUrl = "https://th.bing.com/th/id/OIP.YAVUrxt62F3Xp01B69NQXgHaLg?rs=1&pid=ImgDetMain" },
                    new Libros { Titulo = "Madame Bovary", Autores = "Gustave Flaubert", Precio = 15.99, Descripcion = "Una novela realista que examina las aspiraciones y desilusiones de Emma Bovary en la Francia rural.", ImagenUrl = "https://th.bing.com/th/id/OIP.2oe4TwX7wRZ3vleRzEQEKwHaLH?rs=1&pid=ImgDetMain" }
                }
            },

            new Generos
            {
                TipoGeneros = "Aventura",
                Libros = new List<Libros>
                {

                    new Libros { Titulo = "La isla del Tesoro", Autores = "Robert Louis Stevenson", Precio = 14.99, Descripcion = "Una emocionante historia de aventuras y piratas en busca de un tesoro escondido.", ImagenUrl = "https://th.bing.com/th/id/R.b4cb627f241b11cb229672fcd905aff3?rik=tYKc8LJ2JyZ%2fMg&pid=ImgRaw&r=0" },
                    new Libros { Titulo = "Robinson Crusoe", Autores = "Daniel Defoe", Precio = 12.99, Descripcion = "La clásica historia de un náufrago que lucha por sobrevivir en una isla desierta.", ImagenUrl = "https://th.bing.com/th/id/OIP.zDnmWcvxOm7bw91O2qG3rwAAAA?rs=1&pid=ImgDetMain" },
                    new Libros { Titulo = "El conde de Montecristo", Autores = "Alexandre Dumas", Precio = 19.99, Descripcion = "Un relato de venganza y redención que sigue a Edmond Dantès tras ser traicionado y encarcelado injustamente.", ImagenUrl = "https://th.bing.com/th/id/OIP.i0iXUeJh1-OXFHbArBn4AQHaLO?rs=1&pid=ImgDetMain" },
                    new Libros { Titulo = "Las minas del Rey Salomon", Autores = "H. Rider Haggard", Precio = 16.50, Descripcion = "Una aventura en África que narra la búsqueda de un legendario tesoro perdido.", ImagenUrl = "https://th.bing.com/th/id/R.8d92f173b54dbdf2622d2c90a5176e59?rik=S1bhO%2bggUMt%2f1w&pid=ImgRaw&r=0" },
                    new Libros { Titulo = "Veinte Mil Leguas de Viaje Submarino", Autores = "Julio Verne", Precio = 17.99, Descripcion = "Un fascinante viaje bajo el mar a bordo del submarino Nautilus, liderado por el enigmático Capitán Nemo.", ImagenUrl = "https://th.bing.com/th/id/OIP.McSkhYJ_1gHdZKWnbKw4PAHaLu?w=600&h=950&rs=1&pid=ImgDetMain" },
                    new Libros { Titulo = "Los Tres Mosqueteros", Autores = "Alexandre Dumas", Precio = 18.50, Descripcion = "Una emocionante historia de amistad, lealtad y aventuras en la Francia del siglo XVII.", ImagenUrl = "https://th.bing.com/th/id/R.f0ef86d34cf1750499af8ced2ef6886b?rik=Z4P9%2fkT%2fecmneA&pid=ImgRaw&r=0" }

                }

            }


        };

    public async Task<List<Generos>> Listar(Expression<Func<Generos, bool>> criterio)
	{
		await using var contexto = await DbFactory.CreateDbContextAsync();
		return await contexto.Generos
			.AsNoTracking()
			.Where(criterio)
			.ToListAsync();
	}
}
