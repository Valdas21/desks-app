using desks_api.Application.DTOs;
using desks_api.Domain.Entities;

namespace desks_api.Application.Services.Abstractions
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User?>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<UserReservationsResponse> GetCurrentUserReservationsAsync(int userId);
        Task<UserReservationsResponse> GetPastUserReservationsAsync(int userId);


    }
}
