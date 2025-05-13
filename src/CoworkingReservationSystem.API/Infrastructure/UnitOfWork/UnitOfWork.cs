public class UnitOfWork :  IDisposable
{
    private readonly AppDbContext _context;
    private bool _disposed;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Reservations = new ReservationRepository(_context);
        Rooms = new RoomRepository(_context);
        Users = new UserRepository(_context);
    }

    public ReservationRepository Reservations { get; }
    public RoomRepository Rooms { get; }
    public UserRepository Users { get; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }
        _disposed = true;
    }
}