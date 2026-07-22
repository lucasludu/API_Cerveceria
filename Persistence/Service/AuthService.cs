using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Application.Features._auth.DTOs.Request;
using Application.Features._auth.DTOs.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Persistence.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService ( 
                UserManager<ApplicationUser> userManager, 
                RoleManager<ApplicationRole> roleManager, 
                IConfiguration configuration
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Login a user and generate a JWT token.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<LoginResponse>> LoginUserAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return Response<LoginResponse>.Fail("Usuario o contraseña incorrectos.");
            
            var roles = await _userManager.GetRolesAsync(user!);
            var rol = roles.FirstOrDefault();

            var token = await GenerateJwtTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Token válido por 7 días
            await _userManager.UpdateAsync(user);

            return new Response<LoginResponse>(
                    new LoginResponse { Token = token , UserId = user.Id, Rol = rol!, RefreshToken = refreshToken },
                    $"Usuario {user.Nombre} logueado correctamente."
                );
        }

        /// <summary>
        /// Register a new user and assign a role.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Response<ApplicationUser>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            // Check if the user already exists
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if(existingUser != null)
                return Response<ApplicationUser>.Fail("El correo electrónico ya está en uso.");

            // Create the user
            var user = CreateUserFromRequest(request);
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return Response<ApplicationUser>.Fail (
                    result.Errors.Select(r => r.Description).ToList(), 
                    $"No se pudo crear el usuario {user.UserName}"
                );
            }

            // Assign the role to the user
            var roleResult = await EnsureRoleAndAssignAsync(user, request.Role);
            if (!roleResult.Succeeded)
            {
                return Response<ApplicationUser>.Fail(
                    roleResult.Errors.Select(e => e.Description).ToList(),
                    "No se pudo asignar el rol al usuario.");
            }

            return Response<ApplicationUser>.Success(user);
        }

        /// <summary>
        /// Refreshes the JWT token using a valid RefreshToken.
        /// </summary>
        public async Task<Response<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var principal = GetPrincipalFromExpiredToken(request.Token);
            if (principal == null)
                return Response<LoginResponse>.Fail("Invalid access token or refresh token.");

            var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(email!);

            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Response<LoginResponse>.Fail("Invalid access token or refresh token.");

            var newAccessToken = await GenerateJwtTokenAsync(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            return new Response<LoginResponse>(
                new LoginResponse { Token = newAccessToken, RefreshToken = newRefreshToken, UserId = user.Id, Rol = roles.FirstOrDefault()! },
                "Token refrescado correctamente."
            );
        }

        #region Métodos Privados

        /// <summary>
        /// Generate a JWT token for the user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Create a new user from the registration request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private ApplicationUser CreateUserFromRequest(RegisterUserRequest request)
        {
            return new ApplicationUser
            {
                UserName = $"{request.FirstName}_{request.LastName}",
                Email = request.Email,
                Nombre = request.FirstName,
                Apellido = request.LastName
            };
        }

        /// <summary>
        /// Ensure the role exists and assign it to the user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private async Task<IdentityResult> EnsureRoleAndAssignAsync(ApplicationUser user, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                var createRoleResult = await _roleManager.CreateAsync(new ApplicationRole { Name = role });
                if (!createRoleResult.Succeeded)
                    return createRoleResult;
            }

            return await _userManager.AddToRoleAsync(user, role);
        }

        #endregion
        
        /// <summary>
        /// Generate a random string to be used as a Refresh Token.
        /// </summary>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                ValidateLifetime = false // Here we don't care about the token's expiration
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

    }
}
