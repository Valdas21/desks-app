using desks_api.Application.DTOs;
using desks_api.Domain.Entities;

namespace desks_api.Application.Services.Abstractions
{
    public interface IDeskService
    {
        Task<Desk?> GetByIdAsync(int id);
        Task<IEnumerable<DeskResponse?>> GetAllAsync(DateTime start, DateTime end);
        Task AddAsync(Desk desk);
        Task UpdateAsync(Desk desk);
        Task DeleteAsync(Desk desk);
    }
}
