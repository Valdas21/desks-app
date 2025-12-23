using desks_api.Domain.Enums;
using desks_api.Domain.Entities;

namespace desks_api.Application.DTOs
{
    public class DeskResponse
    {
        public int Id { get; set; }
        public DeskStatus Status { get; set; }
        public bool IsUnderMaintenance { get; set; }
        public CurrentReservationResponse? CurrentReservation { get; set; }

    }
    public class CurrentReservationResponse
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int UserId { get; set; }
    }
}
