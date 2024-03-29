namespace User.Application.Contracts;

public interface IUserRepository
{
    Task<Domain.Models.User> CreateUserAsync(Domain.Models.User model, string password);
    Task<Domain.Models.User?> GetUserByIdAsync(string id);
    Task<Domain.Models.User> GetUserByMailAsync(string mail);
    Task<Domain.Models.User> GetUserByUserNameAsync(string userName);
    Task<bool> GetUserByPhoneAsync(string phone);
    Task<Domain.Models.User> UpdateUserAsync(Domain.Models.User user);
    Task<Domain.Models.User> DeleteUserAsync(Domain.Models.User id);
    Task<bool> CheckOfExists(string field, string checkString);
}