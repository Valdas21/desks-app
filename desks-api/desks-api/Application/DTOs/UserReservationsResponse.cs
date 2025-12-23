namespace desks_api.Application.DTOs
{
    public class UserReservationsResponse
    {
        public UserInfoResponse User { get; set; } = new();
        public List<ReservationResponse> Reservations { get; set; } = new();
    }

    public sealed class UserInfoResponse
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    public sealed class ReservationResponse
    {
        public int Id { get; set; }
        public int DeskId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

