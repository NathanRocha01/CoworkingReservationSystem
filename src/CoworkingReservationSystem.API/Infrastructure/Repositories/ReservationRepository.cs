using Microsoft.EntityFrameworkCore;

public class ReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation> GetByIdAsync(int id)
    {
        return await _context.Reservations
            .Include(r => r.Room)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Reservation>> GetByDateAsync(DateTime date)
    {
        return await _context.Reservations
            .Where(r => r.ReservationDate.Date == date.Date)
            .ToListAsync();
    }

    public async Task AddAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
    }

    public void Update(Reservation reservation)
    {
        _context.Entry(reservation).State = EntityState.Modified;
    }

    public async Task<bool> HasTimeConflictAsync(int roomId, DateTime date, DateTime startTime, DateTime endTime)
    {
        return await _context.Reservations
            .AnyAsync(r => r.RoomId == roomId &&
                          r.ReservationDate.Date == date.Date &&
                          r.Status == ReservationStatus.Confirmed &&
                          ((startTime >= r.StartTime && startTime < r.EndTime) ||
                           (endTime > r.StartTime && endTime <= r.EndTime) ||
                           (startTime <= r.StartTime && endTime >= r.EndTime)));
    }

    public async Task<IEnumerable<Reservation>> GetByUserIdAsync(int userId)
    {
        return await _context.Reservations
            .Include(r => r.Room)
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }
}