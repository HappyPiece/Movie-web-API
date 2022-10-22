using Microsoft.EntityFrameworkCore;
using MovieCatalog.DAL.Models;

namespace MovieCatalog.DAL
{
    public class MovieCatalogDbContext : DbContext
    {
        DbSet<Movie> movies { get; set; }
        public MovieCatalogDbContext(DbContextOptions<MovieCatalogDbContext> options) : base(options)
        {

        }
    }
}
