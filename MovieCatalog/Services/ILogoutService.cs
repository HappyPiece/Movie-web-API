using Microsoft.EntityFrameworkCore;
using MovieCatalog.DAL.Models;

namespace MovieCatalog.Services
{
    public interface ILogoutService
    {
        public Task InvalidateToken(HttpRequest request);

        public Task<bool> IsInvalid(HttpRequest request);
    }
}
