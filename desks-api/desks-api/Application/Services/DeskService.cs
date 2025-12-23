using desks_api.Application.DTOs;
using desks_api.Application.Services.Abstractions;
using desks_api.Domain.Entities;
using desks_api.Domain.Enums;
using desks_api.Domain.Interfaces;

namespace desks_api.Application.Services
{
    public class DeskService : IDeskService
    {
        private readonly IDeskRepository _deskRepository;
        public DeskService(IDeskRepository deskService)
        {
            _deskRepository = deskService;
        }
        public Task AddAsync(Desk desk)
        {
            return _deskRepository.AddAsync(desk);
        }
        public Task DeleteAsync(Desk desk)
        {
            return _deskRepository.DeleteAsync(desk);
        }
        public async Task<IEnumerable<DeskResponse?>> GetAllAsync(DateTime start, DateTime end)
        {
            var desks = await _deskRepository.GetAllAsync(start, end);

            return desks.Select(d =>
            {
                var hasOverlap = d.Reservations.Any();
                return new DeskResponse
                {
                    Id = d.Id,
                    IsUnderMaintenance = d.IsUnderMaintenance,
                    Status = d.IsUnderMaintenance ? DeskStatus.Maintenance
                           : hasOverlap ? DeskStatus.Reserved
                           : DeskStatus.Open,
                    CurrentReservation = d.Reservations
                        .OrderBy(r => r.StartDate)
                        .Select(r => new CurrentReservationResponse
                        {
                            Id = r.Id,
                            StartDate = r.StartDate,
                            EndDate = r.EndDate,
                            FirstName = r.User.FirstName,
                            LastName = r.User.LastName,
                            UserId = r.UserId
                        })
                        .FirstOrDefault()
                };
            }).ToList();
        }
        public Task<Desk?> GetByIdAsync(int id)
        {
            return _deskRepository.GetByIdAsync(id);
        }
        public Task UpdateAsync(Desk desk)
        {
            return _deskRepository.UpdateAsync(desk);
        }
    }
}
