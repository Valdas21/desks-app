using desks_api.Domain.Entities;
using desks_api.Domain.Interfaces;
using desks_api.Application.Services.Abstractions;
using System.Security.Cryptography.Xml;
using Microsoft.OpenApi.Writers;

namespace desks_api.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IDeskRepository _deskRepository;
        public ReservationService(IReservationRepository reservationService, IDeskRepository deskRepository)
        {
            _reservationRepository = reservationService;
            _deskRepository = deskRepository;
        }
        public async Task<Reservation> AddAsync(Reservation reservation)
        {
            if (reservation is null) throw new ArgumentNullException(nameof(reservation));
            if (reservation.EndDate <= reservation.StartDate)
                throw new ArgumentException("EndDate must be after StartDate.");

            var desk = await _deskRepository.GetByIdAsync(reservation.DeskId);
            if (desk is null)
                throw new KeyNotFoundException($"Desk {reservation.DeskId} not found.");

            if (desk.IsUnderMaintenance)
                throw new InvalidOperationException("Desk is under maintenance.");

            var conflict = await _reservationRepository.ExistsOverlapAsync(
                reservation.DeskId,
                reservation.StartDate,
                reservation.EndDate);

            if (conflict)
                throw new InvalidOperationException("Desk is already reserved in that time range.");

             
            await _reservationRepository.AddAsync(reservation);

            return reservation;
        }
        public Task DeleteAsync(Reservation reservation)
        {
            return _reservationRepository.DeleteAsync(reservation);
        }
        public Task<IEnumerable<Reservation?>> GetAllAsync()
        {
            return _reservationRepository.GetAllAsync();
        }
        public Task<Reservation?> GetByIdAsync(int id)
        {
            return _reservationRepository.GetByIdAsync(id);
        }
        public Task UpdateAsync(Reservation reservation)
        {
            return _reservationRepository.UpdateAsync(reservation);
        }
        public async Task CancelWholeAsync(int reservationId, int userId)
        {
            var res = await _reservationRepository.GetByIdAsync(reservationId);
            if (res is null) throw new KeyNotFoundException("Reservation not found.");


            await _reservationRepository.DeleteAsync(res);
        }
        public async Task CancelDayAsync(int reservationId, int userId, DateTime day)
        {
            var res = await _reservationRepository.GetByIdAsync(reservationId);
            if (res is null) throw new KeyNotFoundException("Reservation not found.");

            
            var dayStart = day.Date;
            var dayEnd = dayStart.AddDays(1);

            var overlaps = res.StartDate <= dayEnd && res.EndDate >= dayStart;
            if (!overlaps) throw new InvalidOperationException("Day is not in the reservation date range.");

            //kai pasirinkta diena yra vienintele rezervacijoje
            if (res.StartDate >= dayStart && res.EndDate <= dayEnd)
            {
                await _reservationRepository.DeleteAsync(res);
                return;
            }
            //kai pasirinkta diena yra per viduri rezervacijos
            if (res.StartDate < dayStart && res.EndDate > dayEnd)
            {
                var originalEnd = res.EndDate;

                res.EndDate = dayStart;
                await _reservationRepository.UpdateAsync(res);

                var right = new Reservation
                {
                    UserId = res.UserId,
                    DeskId = res.DeskId,
                    StartDate = dayEnd, 
                    EndDate = originalEnd
                };
                await _reservationRepository.AddAsync(right);

                return;
            }
            //kai pasirinkta diena yra pirma rezervacijoje
            if (res.StartDate >= dayStart && res.StartDate < dayEnd && res.EndDate > dayEnd)
            {
                res.StartDate = dayEnd;
                await _reservationRepository.UpdateAsync(res);
                return;
            }
            //kai pasirinkta diena yra paskutine rezervacijoje
            if (res.StartDate < dayStart && res.EndDate >= dayStart && res.EndDate <= dayEnd)
            {
                res.EndDate = dayStart.AddDays(-1);
                await _reservationRepository.UpdateAsync(res);
                return;
            }
        }
    }
}
