using System.ComponentModel.DataAnnotations.Schema;

public class Reservation
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    [ForeignKey("RoomId")]
    public Room Room { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ReservationStatus Status { get; set; }
    public bool Overlaps(DateTime start, DateTime end)
        => StartTime < end && EndTime > start; 
}