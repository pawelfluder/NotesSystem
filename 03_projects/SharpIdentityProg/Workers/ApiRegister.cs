using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SharpIdentityProg.AAPublic;
using SharpIdentityProg.Registrations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharpIdentityProg.Data;
using SharpIdentityProg.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SharpIdentityProg.Workers;

public class ApiRegister<TUser> : IApiRegister where TUser : class, new()
{
    private readonly IServiceProvider sp;
    private IHttpContextAccessor _contextAccessor;
    private static readonly EmailAddressAttribute _emailAddressAttribute = new();
    private string? confirmEmailEndpointName;
    private IEmailSender<TUser> emailSender;
    private LinkGenerator linkGenerator;
    private readonly object routeGroup;
    private bool _isInit;

    public ApiRegister()
    {
        sp = MyBorder.OutContainer.ServiceProvider;
        confirmEmailEndpointName = null;
        //routeGroup = sp.endpoints.MapGroup("");
    }

    private void Init()
    {
        if (_isInit) return;
        var gg2 = sp.GetHashCode();
        _contextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
        emailSender = sp.GetRequiredService<IEmailSender<TUser>>();
        linkGenerator = sp.GetRequiredService<LinkGenerator>();
        _isInit = true;
    }

    private RegUser ConvertToRegUser(
        string email,
        string password)
    {
        RegUser result = new()
        {
            Email = email, Password = password
        };
        return result;
    }

    public string SignUp(
        string inputEmail,
        string inputPassword)
    {
        var tmp01 = Register(inputEmail, inputPassword)
            .GetAwaiter().GetResult();
        var tmp02 = false;
        if (tmp01 == Results.Ok())
        {
            tmp02 = true;
        }

        string r02 = JsonSerializer.Serialize(tmp02);
        return r02;
    }

    private async Task<IResult> Register(
        string inputEmail,
        string inputPassword)
    {
        Init();
        var registration = ConvertToRegUser(inputEmail, inputPassword);
        var context = _contextAccessor.HttpContext;

        // GENERATED:
        var userManager = sp.GetRequiredService<UserManager<TUser>>();

        if (!userManager.SupportsUserEmail)
        {
            throw new NotSupportedException($"{nameof(Register)} requires a user store with email support.");
        }

        var userStore = sp.GetRequiredService<IUserStore<TUser>>();
        var emailStore = (IUserEmailStore<TUser>)userStore;
        var email = registration.Email;

        if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
        {
            return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
        }

        var user = new TUser();
        await userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await userManager.CreateAsync(user, registration.Password);

        if (!result.Succeeded)
        {
            return CreateValidationProblem(result);
        }

        await SendConfirmationEmailAsync(user, userManager, context, email);
        return TypedResults.Ok();
    }

    private static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        // We expect a single error code and description in the normal case.
        // This could be golfed with GroupBy and ToDictionary, but perf! :P
        Debug.Assert(!result.Succeeded);
        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }

    async Task SendConfirmationEmailAsync(
        TUser user,
        UserManager<TUser> userManager,
        HttpContext context,
        string email,
        bool isChange = false)
    {
        if (confirmEmailEndpointName is null)
        {
            throw new NotSupportedException("No email confirmation endpoint was registered!");
        }

        var code = isChange
            ? await userManager.GenerateChangeEmailTokenAsync(user, email)
            : await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var userId = await userManager.GetUserIdAsync(user);
        var routeValues = new RouteValueDictionary()
        {
            ["userId"] = userId,
            ["code"] = code,
        };

        if (isChange)
        {
            // This is validated by the /confirmEmail endpoint on change.
            routeValues.Add("changedEmail", email);
        }

        var confirmEmailUrl = linkGenerator.GetUriByName(context, confirmEmailEndpointName, routeValues)
                              ?? throw new NotSupportedException(
                                  $"Could not find endpoint named '{confirmEmailEndpointName}'.");

        await emailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(confirmEmailUrl));
    }
}

internal class RegUser
{
    public string Email { get; set; }
    public string Password { get; set; }
}
