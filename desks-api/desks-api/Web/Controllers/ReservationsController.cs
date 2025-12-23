using Microsoft.AspNetCore.Mvc;
using desks_api.Domain.Entities;
using desks_api.Application.Services.Abstractions;

namespace desks_api.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return Ok(reservation);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reservations = await _reservationService.GetAllAsync();
            if (reservations == null || !reservations.Any())
            {
                return NotFound();
            }
            return Ok(reservations);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (reservation is null)
            {
                return BadRequest();
            }
            try
            {
                await _reservationService.AddAsync(reservation);
                return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelWhole(int id, [FromQuery] int userId)
        {
            try
            {
                await _reservationService.CancelWholeAsync(id, userId);
                return NoContent();
            }
            catch (KeyNotFoundException ex) 
            { 
                return NotFound(ex.Message); 
            }
        }

        [HttpPut("{id}/cancel-day")]
        public async Task<IActionResult> CancelDay(int id, [FromQuery] int userId, [FromQuery] DateTime day)
        {
            try
            {
                await _reservationService.CancelDayAsync(id, userId, day);
                return NoContent();
            }
            catch (KeyNotFoundException ex) 
            { 
                return NotFound(ex.Message); 
            }
            catch (InvalidOperationException ex) 
            { 
                return Conflict(ex.Message);
            }
        }
    }
}
