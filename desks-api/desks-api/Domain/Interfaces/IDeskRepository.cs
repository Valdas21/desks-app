using desks_api.Application.DTOs;
using desks_api.Domain.Entities;

namespace desks_api.Domain.Interfaces
{
    public interface IDeskRepository
    {
        Task<Desk?> GetByIdAsync(int id);
        Task<IEnumerable<Desk>> GetAllAsync(DateTime start, DateTime end);
        Task AddAsync(Desk desk);
        Task UpdateAsync(Desk desk);
        Task DeleteAsync(Desk desk);
    }
}
