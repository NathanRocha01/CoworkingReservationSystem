using Microsoft.EntityFrameworkCore;

public class RoomRepository 
{
    private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Room>> GetAllAsync()
    {
        return await _context.Rooms.ToListAsync();
    }

    public async Task<Room> GetByIdAsync(int id)
    {
        return await _context.Rooms.FindAsync(id);
    }

    public async Task<IEnumerable<Room>> GetAvailableAsync(DateTime start, DateTime end)
    {

        var rooms = await _context.Rooms
                                  .Include(r => r.Reservations)
                                  .ToListAsync();

        return rooms.Where(room =>
            room.Reservations.All(res =>
                res.Status == ReservationStatus.Cancelled ||
                res.EndTime <= start || res.StartTime >= end));
    }

    public async Task AddAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
    }

    public void Update(Room room)
    {
        _context.Entry(room).State = EntityState.Modified;
    }

    public void Delete(Room room)
    {
        _context.Rooms.Remove(room);
    }

    public async Task<bool> HasReservationsAsync(int roomId)
    {
        return await _context.Reservations
            .AnyAsync(r => r.RoomId == roomId &&
                          r.ReservationDate >= DateTime.Today);
    }
}