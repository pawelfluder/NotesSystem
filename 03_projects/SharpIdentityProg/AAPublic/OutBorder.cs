using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpIdentityProg.Data;
using System.Security.Claims;
using SharpIdentityProg.Models;
using SharpIdentityProg.Registrations;
using SharpIdentityProg.Services;

namespace SharpIdentityProg.AAPublic
{
    public static class OutBorder
    {
        public static IIdentityService IdentityService()
        {
            bool isReg = MyBorder.IsRegistered;
            IIdentityDbConnectionProvider provider = MyBorder.OutContainer
                .Resolve<IIdentityDbConnectionProvider>();
            IIdentityService service = new IdentityService();
            return service;
        }
        
        public static void AddIdentity(
            this IHostApplicationBuilder builder)
        {
            
        }

        public static void UseIdentity(
            this IEndpointRouteBuilder app)
        {
            app.MapIdentityApi<ApplicationUser>();
            app.MapIdentityApi3<ApplicationUser>();
        }
    }
}
