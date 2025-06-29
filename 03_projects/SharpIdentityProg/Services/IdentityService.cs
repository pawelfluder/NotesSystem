using SharpIdentityProg.AAPublic;

namespace SharpIdentityProg.Services;

public class IdentityService : IIdentityService
{
    public IdentityService(
        IIdentityDbConnectionString connectionString)
    {
    }

    public async Task<bool> SignUp(
        string inputEmail,
        string inputPassword)
    {
        // var userManager = sp.GetRequiredService<UserManager<TUser>>();
        //
        // if (!userManager.SupportsUserEmail)
        // {
        //     throw new NotSupportedException($"{nameof(MapIdentityApi)} requires a user store with email support.");
        // }
        //
        // var userStore = sp.GetRequiredService<IUserStore<TUser>>();
        // var emailStore = (IUserEmailStore<TUser>)userStore;
        // var email = registration.Email;
        //
        // if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
        // {
        //     return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
        // }
        //
        // var user = new TUser();
        // await userStore.SetUserNameAsync(user, email, CancellationToken.None);
        // await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        // var result = await userManager.CreateAsync(user, registration.Password);
        //
        // if (!result.Succeeded)
        // {
        //     return CreateValidationProblem(result);
        // }
        //
        // await SendConfirmationEmailAsync(user, userManager, context, email);
        // return TypedResults.Ok();
        return false;
    }
}
