using desks_api.Domain.Entities;

namespace desks_api.Domain.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation?> GetByIdAsync(int id);
        Task<IEnumerable<Reservation?>> GetAllAsync();
        Task AddAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(Reservation reservation);
        Task<bool> ExistsOverlapAsync(int deskId, DateTime start, DateTime end);
        Task<List<Reservation>> GetByUserIdAsync(int userId);
    }
}
