namespace MBAPI_HW.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(string account, string role);
    }
}
