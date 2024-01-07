using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper.Authentication.Interfaces
{
    public interface ISignInKeys
    {
        Task<IEnumerable<SecurityKey>> GetSigninKeys(string authority);
    }
}
