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
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public ReservationStatus Status { get; set; }
}