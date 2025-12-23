using desks_api.Domain.Entities;

namespace desks_api.Application.Services.Abstractions
{
    public interface IReservationService
    {
        Task<Reservation?> GetByIdAsync(int id);
        Task<IEnumerable<Reservation?>> GetAllAsync();
        Task<Reservation> AddAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(Reservation reservation);
        Task CancelWholeAsync(int reservationId, int userId);
        Task CancelDayAsync(int reservationId, int userId, DateTime day);
    }
}
