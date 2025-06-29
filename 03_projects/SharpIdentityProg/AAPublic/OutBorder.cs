using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharpIdentityProg.Data;
using System.Security.Claims;
using SharpIdentityProg.Services;

namespace SharpIdentityProg.AAPublic
{
    public static class OutBorder
    {
        public static IIdentityService IdentityService(
            IIdentityDbConnectionString connectionString)
        {
            IIdentityService service = new IdentityService(connectionString);
            return service;
        }
        
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
            app.MapIdentityApi2<ApplicationUser>();

            // app.MapPost("/logout", async (SignInManager<ApplicationUser> signInManager) =>
            // {
            //
            //     await signInManager.SignOutAsync();
            //     return Results.Ok();
            //
            // }).RequireAuthorization();
            //
            // app.MapGet("/pingauth", (ClaimsPrincipal user) =>
            // {
            //     var email = user.FindFirstValue(ClaimTypes.Email); // get the user's email from the claim
            //     return Results.Json(new { Email = email }); ; // return the email as a plain text response
            // }).RequireAuthorization();
        }
    }
}
