using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoworkingReservationSystem.Web.Models.ViewModels
{
    public class RoomAvailabilityViewModel
    {
        [Required(ErrorMessage = "Data inicial obrigatória")]
        [Display(Name = "Data de Início")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; } = DateTime.Now.AddHours(1);

        [Required(ErrorMessage = "Data final obrigatória")]
        [Display(Name = "Data de Término")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [DateGreaterThan("StartDate", ErrorMessage = "O término deve ser após o início")]
        public DateTime EndDate { get; set; } = DateTime.Now.AddHours(3);

        public List<AvailableRoomDto> AvailableRooms { get; set; } = new();

        // Validação customizada para data final > data inicial
        public class DateGreaterThanAttribute : ValidationAttribute
        {
            private readonly string _comparisonProperty;

            public DateGreaterThanAttribute(string comparisonProperty)
            {
                _comparisonProperty = comparisonProperty;
            }

            protected override ValidationResult IsValid(object value, ValidationContext context)
            {
                var currentValue = (DateTime)value;
                var property = context.ObjectType.GetProperty(_comparisonProperty);

                if (property == null)
                    throw new ArgumentException("Propriedade não encontrada");

                var comparisonValue = (DateTime)property.GetValue(context.ObjectInstance);

                return currentValue > comparisonValue
                    ? ValidationResult.Success
                    : new ValidationResult(ErrorMessage);
            }
        }
    }

    public class AvailableRoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal HourlyRate { get; set; }

        // Propriedade calculada
        public string FormattedPeriod => $"{Capacity}pax - {HourlyRate}/h";
        public bool IsPremium { get; set; }
    }
}