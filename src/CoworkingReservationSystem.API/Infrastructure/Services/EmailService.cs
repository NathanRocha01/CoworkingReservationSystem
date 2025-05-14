public class EmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendReservationConfirmationAsync(Reservation reservation)
    {
        _logger.LogInformation(
            $"[E-mail Simulado] Para: {reservation.User.Email}\n" +
            $"Assunto: Confirma??o de Reserva\n" +
            $"Corpo: Reserva confirmada para {reservation.Room.Name} " +
            $"em {reservation.ReservationDate:d} das {reservation.StartTime} ?s {reservation.EndTime}."
        );

        await Task.CompletedTask;
    }

    public async Task SendReservationCancellationAsync(Reservation reservation)
    {
        _logger.LogInformation(
            $"[E-mail Simulado] Cancelamento para: {reservation.User.Email}\n" +
            $"Reserva ID: {reservation.Id} cancelada com sucesso."
        );

        await Task.CompletedTask;
    }
}