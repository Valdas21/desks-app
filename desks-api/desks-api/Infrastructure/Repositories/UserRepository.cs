using desks_api.Application.DTOs;
using desks_api.Domain.Entities;
using desks_api.Domain.Interfaces;
using desks_api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace desks_api.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<User?>> GetAllAsync()
        {
            return await Task.FromResult(_context.Users.ToList());
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public Task<User?> GetByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<List<User>> GetAllWithReservationsAsync()
        {
            return await _context.Users
                .Include(u => u.Reservations)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
