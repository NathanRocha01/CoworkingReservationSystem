using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoworkingReservationSystem.Web.Models.ViewModels
{
    public class ReservationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Sala é obrigatória.")]
        public string RoomName { get; set; }

        [Required(ErrorMessage = "Data e hora são obrigatórias.")]
        [FutureDate(ErrorMessage = "A data deve ser futura.")]
        public DateTime ReservationDate { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;

        public List<SelectListItem> AvailableRooms { get; set; } = new List<SelectListItem>();

        public enum ReservationStatus
        {
            Confirmed = 1,
            Canceled = 2
        }

        public class FutureDateAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                return value is DateTime date && date > DateTime.Now;
            }
        }

    }
}