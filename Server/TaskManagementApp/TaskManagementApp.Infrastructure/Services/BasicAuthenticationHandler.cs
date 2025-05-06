using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace TaskManagementApp.Infrastructure.Services
{
    //public class BasicAuthenticationHandler_delete : AuthenticationHandler<AuthenticationSchemeOptions>
    //{
    //    private readonly IUserRepository _userRepository;

    //    public BasicAuthenticationHandler(
    //        IOptionsMonitor<AuthenticationSchemeOptions> options,
    //        ILoggerFactory logger,
    //        UrlEncoder encoder,
    //        ISystemClock clock,
    //        IUserRepository userRepository)
    //        : base(options, logger, encoder, clock)
    //    {
    //        _userRepository = userRepository;
    //    }

    //    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    //    {
    //        if (!Request.Headers.ContainsKey("Authorization"))
    //            return AuthenticateResult.Fail("Missing Authorization Header");

    //        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
    //        var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? "");
    //        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
    //        var username = credentials[0];
    //        var password = credentials[1];

    //        var user = await _userRepository.GetByUsernameAsync(username);
    //        if (user == null || user.Password != password)
    //            return AuthenticateResult.Fail("Invalid credentials");

    //        var claims = new[] { new Claim(ClaimTypes.Name, username) };
    //        var identity = new ClaimsIdentity(claims, Scheme.Name);
    //        var principal = new ClaimsPrincipal(identity);
    //        var ticket = new AuthenticationTicket(principal, Scheme.Name);

    //        return AuthenticateResult.Success(ticket);
    //    }
    //}
}
