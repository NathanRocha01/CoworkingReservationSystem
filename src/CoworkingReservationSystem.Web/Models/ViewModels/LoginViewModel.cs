using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace CoworkingReservationSystem.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-mail � obrigat�rio.")]
        [EmailAddress(ErrorMessage = "E-mail inv�lido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha � obrigat�ria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}