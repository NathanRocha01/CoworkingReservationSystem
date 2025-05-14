using System.ComponentModel.DataAnnotations;

public class ReservationRequest
{
    [Required]
    public int RoomId { get; set; }

    [Required, FutureDate(ErrorMessage = "Data deve ser futura")]
    public DateTime ReservationDate { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required, EndTimeAfterStart]
    public DateTime EndTime { get; set; }

}

public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        return value is DateTime date && date > DateTime.Today;
    }
}

public class EndTimeAfterStartAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext context)
    {
        var instance = context.ObjectInstance as ReservationRequest;
        if (instance == null || value is not DateTime endTime)
            return ValidationResult.Success;

        return endTime > instance.StartTime
            ? ValidationResult.Success
            : new ValidationResult("O hor�rio final deve ser ap�s o hor�rio inicial");
    }
}