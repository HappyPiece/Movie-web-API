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
        private readonly MovieCatalogDbContext _context;
        private readonly JwtSecurityTokenHandler _handler;

        public LogoutService(MovieCatalogDbContext context)
        {
            _handler = new JwtSecurityTokenHandler();
            _context = context;
        }

        public async Task InvalidateToken(HttpRequest request)
        {
            var jwt = ExtractJwtToken(request);
            await _context.CompromisedTokens.AddAsync(new CompromisedToken
            { 
                Token = jwt.ToString(),
                ExpiryTime = jwt.ValidTo
            } );
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsInvalid(HttpRequest request)
        {
            var jwt = ExtractJwtToken(request);
            return await _context.CompromisedTokens.AnyAsync(x => x.Token == jwt.ToString());
        }

        private JwtSecurityToken ExtractJwtToken(HttpRequest request)
        {
            return _handler.ReadJwtToken(Regex.Match(request.Headers[HeaderNames.Authorization], HeaderRegex).Groups["token"].Value);
        }
    }
}
