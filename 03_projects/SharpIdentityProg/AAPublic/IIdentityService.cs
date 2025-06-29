namespace SharpIdentityProg.AAPublic;

public interface IIdentityService
{
    public Task<bool> SignUp(
        string email,
        string password);
}
