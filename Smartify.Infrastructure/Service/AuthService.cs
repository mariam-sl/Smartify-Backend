using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smartify.Application.Common;
using Smartify.Application.DTO.Auth;
using Smartify.Application.Exceptions;
using Smartify.Application.IService;
using Smartify.Domain.Constants;
using Smartify.Infrastructure.Data;
using Smartify.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService,
            AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _context = context;
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow,
                UserName = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to register user");
            }

            await _userManager.AddToRoleAsync(user, Roles.Student);
            var roles = await _userManager.GetRolesAsync(user);

            var tokenUser = new TokenUser
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = user.FullName
            };
            var accessToken = _tokenService.CreateAccessToken(tokenUser, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();
            refreshToken.UserId = user.Id;
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();


            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new UnauthorizedException("Invalid email or password");
            var roles = await _userManager.GetRolesAsync(user);

            var tokenUser = new TokenUser
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = user.FullName
            };

            var accessToken = _tokenService.CreateAccessToken(tokenUser, roles);

            var refreshToken = _tokenService.GenerateRefreshToken();
            refreshToken.UserId = user.Id;
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };
        }

        public async Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                 .FirstOrDefaultAsync(x => x.Token == refreshToken);
            if (token == null || !token.IsActive)
            {
                throw new Exception("Invalid refresh token");
            }
            //revoke old token(rotation)
            token.Revoked = DateTime.UtcNow;

            // GET USER VIA USERMANAGER (IMPORTANT)
            var user = await _userManager.FindByIdAsync(token.UserId);

            if (user == null)
                throw new Exception("User not found");

            var roles = await _userManager.GetRolesAsync(user);
            var tokenUser = new TokenUser
            {
                Id = user.Id,
                Email = user.Email!,
                FullName = user.FullName
            };
            var newAccessToken = _tokenService.CreateAccessToken(tokenUser, roles);

            var newRefreshToken = _tokenService.GenerateRefreshToken();
            newRefreshToken.UserId = user.Id;

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponseDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

        }
    }
}
