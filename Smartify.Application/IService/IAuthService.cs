using Smartify.Application.DTO.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.IService
{
    public interface IAuthService
    {
            Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto);
            Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
            Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken);

    }
}
