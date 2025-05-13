using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoworkingReservationSystem.Web.Models.ViewModels
{
    public class ReservationViewModel
    {
        [Required(ErrorMessage = "Sala � obrigat�ria.")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Data e hora s�o obrigat�rias.")]
        [FutureDate(ErrorMessage = "A data deve ser futura.")]
        public DateTime ReservationDate { get; set; }

        public List<SelectListItem> AvailableRooms { get; set; } = new List<SelectListItem>();

        public class FutureDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                return value is DateTime date && date > DateTime.Now;
            }
        }
    }
}