using System.ComponentModel.DataAnnotations;
namespace CoworkingReservationSystem.Web.Models.ViewModels
{
    public class RoomViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome da sala é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Capacidade é obrigatória.")]
        [Range(1, 20, ErrorMessage = "Capacidade deve ser entre 1 e 20.")]
        public int Capacity { get; set; }
    }
}