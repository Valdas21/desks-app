using desks_api.Domain.Interfaces;
using desks_api.Domain.Entities;
using desks_api.Application.Services.Abstractions;
using desks_api.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace desks_api.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IReservationRepository _reservationRepository;
        public UserService(IUserRepository userService, IReservationRepository reservationRepository)
        {
            _userRepository = userService;
            _reservationRepository = reservationRepository;
        }
        public Task AddAsync(User user)
        {
            return _userRepository.AddAsync(user);
        }
        public Task DeleteAsync(User user)
        {
            return _userRepository.DeleteAsync(user);
        }
        public Task<IEnumerable<User?>> GetAllAsync()
        {
            return _userRepository.GetAllAsync();
        }
        public Task<User?> GetByIdAsync(int id)
        {
            return _userRepository.GetByIdAsync(id);
        }
        public Task UpdateAsync(User user)
        {
            return _userRepository.UpdateAsync(user);
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Email and password are required.");

            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null)
                throw new KeyNotFoundException("User not found.");

            if (user.Password != request.Password)
                throw new InvalidOperationException("Invalid login.");

            return new LoginResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
        public async Task<UserReservationsResponse> GetCurrentUserReservationsAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            var now = DateTime.UtcNow;
            var reservations = await _reservationRepository.GetByUserIdAsync(userId);

            return new UserReservationsResponse
            {
                User = new UserInfoResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                },
                Reservations = reservations
                    .Where(r => r.EndDate >= now)
                    .OrderBy(r => r.StartDate)
                    .Select(r => new ReservationResponse
                    {
                        Id = r.Id,
                        DeskId = r.DeskId,
                        StartDate = r.StartDate,
                        EndDate = r.EndDate
                    })
                    .ToList()
            };
        }

        public async Task<UserReservationsResponse> GetPastUserReservationsAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            var now = DateTime.UtcNow;
            var reservations = await _reservationRepository.GetByUserIdAsync(userId);

            return new UserReservationsResponse
            {
                User = new UserInfoResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                },
                Reservations = reservations
                    .Where(r => r.EndDate < now)
                    .OrderByDescending(r => r.EndDate)
                    .Select(r => new ReservationResponse
                    {
                        Id = r.Id,
                        DeskId = r.DeskId,
                        StartDate = r.StartDate,
                        EndDate = r.EndDate
                    })
                    .ToList()
            };
        }
    }
}
