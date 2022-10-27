using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using MovieCatalog.DAL;
using MovieCatalog.DAL.Models;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace MovieCatalog.Services
{
    public class LogoutService : ILogoutService
    {
        public static string HeaderRegex = @"Bearer (?<token>.*)";
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        private readonly JwtSecurityTokenHandler _handler;
        private readonly IServiceScopeFactory _scopeFactory;

        public LogoutService(IServiceScopeFactory scopeFactory)
        {
            _handler = new JwtSecurityTokenHandler();
            _scopeFactory = scopeFactory;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            Task.Factory.StartNew(DeleteExpiredTokens, _cancellationToken);
        }

        ~LogoutService()
        {
            _cancellationTokenSource.Cancel();
        }

        public async Task InvalidateToken(HttpRequest request)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MovieCatalogDbContext>();
                var jwt = ExtractJwtToken(request);
                await context.CompromisedTokens.AddAsync(new CompromisedToken
                {
                    Token = jwt.ToString(),
                    ExpiryTime = jwt.ValidTo
                });
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsInvalid(HttpRequest request)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MovieCatalogDbContext>();
                var jwt = ExtractJwtToken(request);
                return await context.CompromisedTokens.AnyAsync(x => x.Token == jwt.ToString());
            }
        }

        private async Task DeleteExpiredTokens()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    var context = scope.ServiceProvider.GetRequiredService<MovieCatalogDbContext>();
                    context.CompromisedTokens.RemoveRange(await context.CompromisedTokens.Where(x => x.ExpiryTime < DateTime.UtcNow).ToListAsync());
                    await context.SaveChangesAsync();
                    
                    Console.WriteLine("Kupil muzhik shlyapu, a ona yemu expired tokeny udalila");
                    await Task.Delay(1000*60);
                }
            }
        }

        private JwtSecurityToken ExtractJwtToken(HttpRequest request)
        {
            return _handler.ReadJwtToken(Regex.Match(request.Headers[HeaderNames.Authorization], HeaderRegex).Groups["token"].Value);
        }
    }
}
