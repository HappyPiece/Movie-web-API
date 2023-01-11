using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using MovieCatalog.Controllers;
using MovieCatalog.DAL;
using MovieCatalog.Properties;
using MovieCatalog.Services;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace MovieCatalog
{
    public class AdvancedJwtBearerHandler : JwtBearerHandler
    {
        public static string AdvancedJwtBearerScheme = "AdvancedJwtBearer";

        private ILogoutService _logoutService;

        public AdvancedJwtBearerHandler
        (
            IOptionsMonitor<JwtBearerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ILogoutService logoutService
        ) : base(options, logger, encoder, clock)
        {
            _logoutService = logoutService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                return AuthenticateResult.Fail(GenericConstants.MissingAuthHeader);
            }

            if (await _logoutService.IsInvalid(Request))
            {
                return AuthenticateResult.Fail(GenericConstants.InvalidToken);
            }

            AuthenticateResult result = await base.HandleAuthenticateAsync();
            return result;
        }
    }

}
