using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper.Service.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponse> GetToken(string scope);
    }
}
