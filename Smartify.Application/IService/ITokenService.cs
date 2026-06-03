using Smartify.Application.Common;
using Smartify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Application.IService
{
    public interface ITokenService
    {

        string CreateAccessToken(TokenUser user,IList<string> roles);

        RefreshToken GenerateRefreshToken();

    }
}
