using System.ComponentModel.DataAnnotations;

public class RoomRequest
{
    [Required, MaxLength(50)]
    public string Name { get; set; }

    [Required, Range(1, 20)]
    public int Capacity { get; set; }
}