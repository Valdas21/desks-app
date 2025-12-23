using desks_api.Domain.Entities;
using desks_api.Domain.Interfaces;
using desks_api.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using desks_api.Application.DTOs;
using desks_api.Domain.Enums;


namespace desks_api.Infrastructure.Repositories
{
    public class DeskRepository : IDeskRepository
    {
        private readonly AppDbContext _context;

        public DeskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Desk desk)
        {
            await _context.Desks.AddAsync(desk);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Desk desk)
        {
            _context.Desks.Remove(desk);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Desk>> GetAllAsync(DateTime start, DateTime end)
        {
            return await _context.Desks
                .Include(d => d.Reservations.Where(r => r.StartDate < end && r.EndDate > start))
                    .ThenInclude(r => r.User)
                .ToListAsync();
        }

        public async Task<Desk?> GetByIdAsync(int id)
        {
            return await _context.Desks.FindAsync(id);
        }

        public async Task UpdateAsync(Desk desk)
        {
            _context.Desks.Update(desk);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
        }
    }
}
