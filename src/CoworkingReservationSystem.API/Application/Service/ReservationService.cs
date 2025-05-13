public class ReservationService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly EmailService _emailService;

    public ReservationService(
        UnitOfWork unitOfWork,
        EmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task<Result> CreateReservationAsync(ReservationRequest request, int userId)
    {

        var roomExists = await _unitOfWork.Rooms.GetByIdAsync(request.RoomId);
        if (roomExists == null)
            return Result.Failure("Sala não encontrada");

        if (await _unitOfWork.Reservations.HasTimeConflictAsync(
            request.RoomId,
            request.ReservationDate,
            request.StartTime,
            request.EndTime))
        {
            return Result.Failure("Conflito de horário com outra reserva");
        }

        var reservation = new Reservation
        {
            RoomId = request.RoomId,
            UserId = userId,
            ReservationDate = request.ReservationDate,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Status = ReservationStatus.Confirmed
        };

        await _unitOfWork.Reservations.AddAsync(reservation);
        await _unitOfWork.CompleteAsync();

        await _emailService.SendReservationConfirmationAsync(reservation);

        return Result.Success(reservation.Id);
    }

    public async Task<Result> CancelReservationAsync(int id, int userId)
    {
        var reservation = await _unitOfWork.Reservations.GetByIdAsync(id);

        if (reservation == null)
            return Result.Failure("Reserva não existe");

        if (reservation.UserId != userId)
            return Result.Failure("Não autorizado");

        if ((reservation.ReservationDate - DateTime.Now).TotalHours < 24)
            return Result.Failure("Cancelamento requer 24h de antecedência");

        reservation.Status = ReservationStatus.Cancelled;
        _unitOfWork.Reservations.Update(reservation);
        await _unitOfWork.CompleteAsync();

        await _emailService.SendReservationCancellationAsync(reservation);

        return Result.Success();
    }

    public async Task<IEnumerable<ReservationResponse>> GetUserReservationsAsync(int userId)
    {
        var reservations = await _unitOfWork.Reservations
            .GetByUserIdAsync(userId);

        return reservations.Select(r => new ReservationResponse
        {
            Id = r.Id,
            RoomName = r.Room.Name,
            ReservationDate = r.ReservationDate,
            StartTime = r.StartTime,
            EndTime = r.EndTime,
            Status = r.Status.ToString()
        });
    }
}
