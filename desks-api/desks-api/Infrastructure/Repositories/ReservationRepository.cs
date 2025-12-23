using desks_api.Domain.Entities;
using desks_api.Domain.Interfaces;
using desks_api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace desks_api.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;

        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        public async Task<IEnumerable<Reservation?>> GetAllAsync()
        {
            return await _context.Reservations.ToListAsync();
        }

        public async Task AddAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }
        public Task<bool> ExistsOverlapAsync(int deskId, DateTime start, DateTime end)
        {
            return _context.Reservations.AnyAsync(r =>
                r.DeskId == deskId &&
                r.StartDate < end && r.EndDate > start 
            );
        }
        public async Task<List<Reservation>> GetByUserIdAsync(int userId)
        {
            return await _context.Reservations
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
    }
}
