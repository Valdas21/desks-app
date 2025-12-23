using desks_api.Application.DTOs;
using desks_api.Domain.Entities;

namespace desks_api.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User?>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetAllWithReservationsAsync();

    }
}
