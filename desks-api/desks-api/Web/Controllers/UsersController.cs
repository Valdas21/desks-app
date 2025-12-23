using Microsoft.AspNetCore.Mvc;
using desks_api.Application.Services.Abstractions;
using desks_api.Domain.Entities;
using desks_api.Application.DTOs;

namespace desks_api.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            if (users == null || !users.Any())
            {
                return NotFound();
            }
            return Ok(users);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Create(User user)
        {
            if (user is null)
            {
                return BadRequest();
            }
            await _userService.AddAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var result = await _userService.LoginAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        [HttpGet("{id}/Reservations-current")]
        public async Task<IActionResult> GetWithAcitveReservations(int id)
        {
            var users = await _userService.GetCurrentUserReservationsAsync(id); ;
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }
        [HttpGet("{id}/Reservations-past")]
        public async Task<IActionResult> GetWithPastReservations(int id)
        {
            var users = await _userService.GetPastUserReservationsAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }
    }
}
