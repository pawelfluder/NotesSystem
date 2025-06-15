using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpIdentityProg.Data;
using System.Security.Claims;

namespace SharpIdentityProg.AAPublic
{
    public static class OutBorder
    
    {
        public static void AddIdentity(
            this IHostApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>();
            builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddAuthorization();
        }

        public static void UseIdentity(
            this IEndpointRouteBuilder app)
        {
            app.CustomMapIdentityApi<ApplicationUser>();
        }
    }
}
