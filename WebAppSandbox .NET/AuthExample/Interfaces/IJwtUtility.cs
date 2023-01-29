namespace AuthExample.Interfaces
{
    public interface IJwtUtility
    {
        string CreateUserAuthToken(string userId);

        bool ValidateJwtSources(string userId, string authorization);
    }
}
