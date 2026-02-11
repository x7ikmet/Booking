namespace Booking.Domain.Users;

public interface IUserReposotory
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    void Add(User user);
}
