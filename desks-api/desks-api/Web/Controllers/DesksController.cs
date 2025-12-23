using Microsoft.AspNetCore.Mvc;
using desks_api.Domain.Entities;
using desks_api.Application.Services.Abstractions;
using desks_api.Application.DTOs;

namespace desks_api.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DesksController : ControllerBase
    {
        private readonly IDeskService _deskService;
        public DesksController(IDeskService deskService)
        {
            _deskService = deskService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var desk = await _deskService.GetByIdAsync(id);
            if (desk == null)
            {
                return NotFound();
            }
            return Ok(desk);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var desks = await _deskService.GetAllAsync(start, end);
            if (desks == null || !desks.Any())
            {
                return NotFound();
            }
            return Ok(desks);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Desk desk)
        {
            if (desk is null)
            {
                return BadRequest();
            }
            await _deskService.UpdateAsync(desk);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Desk desk)
        {
            if (desk is null)
            {
                return BadRequest();
            }
            await _deskService.AddAsync(desk);
            return CreatedAtAction(nameof(GetById), new { id = desk.Id }, desk);
        }
    }
}
