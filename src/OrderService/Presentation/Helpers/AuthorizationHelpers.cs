using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace OrderService.Presentation.Helpers
{
    public static class AuthorizationHelpers
    {
        public static async Task<bool> AuthorizeResourceAsync<T>(this IAuthorizationService authService, ClaimsPrincipal user, T resourse, string policyName)
        {
            var result = await authService.AuthorizeAsync(user, resourse, policyName);
            return result.Succeeded;
        }
        
     

        
    }
}
