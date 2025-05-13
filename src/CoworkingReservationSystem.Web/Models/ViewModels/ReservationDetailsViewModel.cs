using System;
using System.ComponentModel.DataAnnotations;

namespace CoworkingReservationSystem.Web.Models.ViewModels
{
    public class ReservationDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Sala")]
        public string RoomName { get; set; }

        [Display(Name = "Capacidade")]
        public int RoomCapacity { get; set; }

        [Display(Name = "Início")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Término")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Duração")]
        public string Duration => $"{(EndDate - StartDate).TotalHours:F1} horas";

        [Display(Name = "Status")]
        public string Status { get; set; } // "Confirmada", "Cancelada", "Concluída"

        [Display(Name = "Usuário")]
        public string UserName { get; set; }

        [Display(Name = "E-mail")]
        public string UserEmail { get; set; }

        [Display(Name = "Criada em")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime CreatedAt { get; set; }
        public bool CanCancel => Status == "Confirmada" && StartDate > DateTime.Now.AddHours(24);
        public bool CanEdit => Status == "Confirmada" && StartDate > DateTime.Now.AddHours(1);
    }
}