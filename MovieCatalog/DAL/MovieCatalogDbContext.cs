using Microsoft.EntityFrameworkCore;
using MovieCatalog.DAL.Models;

namespace MovieCatalog.DAL
{
    public class MovieCatalogDbContext : DbContext
    {
        DbSet<Movie> movies { get; set; }
        DbSet<User> users { get; set; }
        DbSet<Review> reviews { get; set; }
        DbSet<Genre> genres { get; set; }
        public MovieCatalogDbContext(DbContextOptions<MovieCatalogDbContext> options) : base(options)
        {

        }
    }
}
