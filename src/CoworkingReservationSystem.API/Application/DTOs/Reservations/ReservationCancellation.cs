using System.ComponentModel.DataAnnotations;

public class ReservationCancellation
{
    [Required]
    public int ReservationId { get; set; }

}