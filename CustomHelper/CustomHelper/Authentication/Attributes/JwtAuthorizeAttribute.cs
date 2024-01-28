using CustomHelper.Authentication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomHelper.Authentication.NewFolder;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CustomHelper.Authentication.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class JwtAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public string[] _policies { get; }

        public JwtAuthorizeAttribute()
        {
            _policies = new string[0];
        }

        public JwtAuthorizeAttribute(
            params string[] policies)
        {
            _policies = policies;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (_policies.Any() && !await ValidateByPolicies(context))
            {
                context.Result = new ForbidResult();
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;

            var jwtToken = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtToken))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var signInKeys = (ISignInKeys)context.HttpContext.RequestServices.GetService(typeof(ISignInKeys));

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(jwtToken, await ValidationParameters(signInKeys, audience, issuer), out _);

                return;
            }
            catch (System.Exception)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        private async Task<TokenValidationParameters> ValidationParameters(ISignInKeys signInKeys, string audience, string issuer)
        {
            var key = await signInKeys.GetSigninKeys(issuer);

            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key.FirstOrDefault(),
                ValidateIssuer = !string.IsNullOrEmpty(issuer),
                ValidIssuer = issuer,
                ValidateAudience = !string.IsNullOrEmpty(audience),
                ValidAudience = audience,
                ValidateLifetime = true,
            };
        }

        private async Task<bool> ValidateByPolicies(AuthorizationFilterContext context)
        {
            var isAuthorized = false;
            var authService = context.HttpContext.RequestServices.GetService(typeof(IAuthorizationService)) as IAuthorizationService;

            foreach (var policy in _policies)
            {
                var authorizationResult = await authService.AuthorizeAsync(context.HttpContext.User, context, policy);
                if (authorizationResult.Succeeded)
                {
                    isAuthorized = true;
                    break;
                }
            }

            return isAuthorized;
        }
    }
}
