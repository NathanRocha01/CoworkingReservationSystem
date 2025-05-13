public class ReservationResponse
{
    public int Id { get; set; }
    public string RoomName { get; set; }
    public DateTime ReservationDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Status { get; set; }
}