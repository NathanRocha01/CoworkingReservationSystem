using System.ComponentModel.DataAnnotations;

namespace CoworkingReservationSystem.Web.Models.ViewModels
{
    public class ReservationCancelViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Status { get; set; } = "Cancelada";
    }
}