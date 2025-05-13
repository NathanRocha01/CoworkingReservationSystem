using System.ComponentModel.DataAnnotations;

public class ReservationRequest
{
    [Required]
    public int RoomId { get; set; }

    [Required, FutureDate(ErrorMessage = "Data deve ser futura")]
    public DateTime ReservationDate { get; set; }

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required, EndTimeAfterStart]
    public TimeSpan EndTime { get; set; }

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
        if (instance == null || value is not TimeSpan endTime)
            return ValidationResult.Success;

        return endTime > instance.StartTime
            ? ValidationResult.Success
            : new ValidationResult("O horário final deve ser após o horário inicial");
    }
}