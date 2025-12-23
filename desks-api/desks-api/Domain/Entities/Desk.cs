using desks_api.Domain.Enums;

namespace desks_api.Domain.Entities
{
    public class Desk
    {
        public int Id { get; set; }
        public bool IsUnderMaintenance { get; set; }
        public string? Message { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
}
}
