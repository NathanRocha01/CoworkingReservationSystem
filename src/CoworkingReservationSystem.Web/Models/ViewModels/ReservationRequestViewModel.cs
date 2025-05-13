using static CoworkingReservationSystem.Web.Models.ViewModels.ReservationViewModel;
using System.ComponentModel.DataAnnotations;

namespace CoworkingReservationSystem.Web.Models.ViewModels
{
    public class ReservationRequestViewModel
    {
        [Required(ErrorMessage = "A sala é obrigatória.")]
        [Display(Name = "Sala")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "A data e hora de início são obrigatórias.")]
        [Display(Name = "Início")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [FutureDate(ErrorMessage = "A reserva deve começar no futuro.")]
        public DateTime StartDateTime { get; set; } = DateTime.Now.AddHours(1);

        [Required(ErrorMessage = "A data e hora de término são obrigatórias.")]
        [Display(Name = "Término")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [EndDateAfterStart(ErrorMessage = "O término deve ser após o início.")]
        public DateTime EndDateTime { get; set; } = DateTime.Now.AddHours(2);

        public class FutureDateAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is DateTime date)
                {
                    if (date < DateTime.Now.AddHours(1))
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
                return ValidationResult.Success;
            }
        }
        public class EndDateAfterStartAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var model = (ReservationRequestViewModel)validationContext.ObjectInstance;
                if (model.EndDateTime <= model.StartDateTime)
                {
                    return new ValidationResult(ErrorMessage);
                }
                return ValidationResult.Success;
            }
        }
    }
}
