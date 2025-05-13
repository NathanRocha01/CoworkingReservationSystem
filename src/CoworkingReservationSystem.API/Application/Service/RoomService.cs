public class RoomService
{
    private readonly UnitOfWork _unitOfWork;

    public RoomService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<RoomResponse>> GetAllAsync()
    {
        var rooms = await _unitOfWork.Rooms.GetAllAsync();
        return rooms.Select(r => new RoomResponse
        {
            Id = r.Id,
            Name = r.Name,
            Capacity = r.Capacity,
        });
    }

    public async Task<Result> CreateRoomAsync(RoomRequest request)
    {
        var room = new Room
        {
            Name = request.Name,
            Capacity = request.Capacity,
        };

        await _unitOfWork.Rooms.AddAsync(room);
        await _unitOfWork.CompleteAsync();

        return Result.Success(room.Id);
    }

    public async Task<Result> UpdateRoomAsync(int id, RoomRequest request)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(id);
        if (room == null)
            return Result.Failure("Sala não encontrada");

        room.Name = request.Name;
        room.Capacity = request.Capacity;

        _unitOfWork.Rooms.Update(room);
        await _unitOfWork.CompleteAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteRoomAsync(int id)
    {
        var room = await _unitOfWork.Rooms.GetByIdAsync(id);
        if (room == null)
            return Result.Failure("Sala não encontrada");

        if (await _unitOfWork.Rooms.HasReservationsAsync(id))
            return Result.Failure("Sala possui reservas futuras");

        _unitOfWork.Rooms.Delete(room);
        await _unitOfWork.CompleteAsync();

        return Result.Success();
    }
}