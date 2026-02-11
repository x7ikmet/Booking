
using Booking.Domain.Abstractions;
using Booking.Domain.Users.Events;

namespace Booking.Domain.Users;

public sealed class User: Entity
{
    private User(Guid id, FirstName firstName, string lastName, string email) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public FirstName FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    public static User Create(FirstName firstName, string lastName, string email)
    {
        var user = new User(Guid.NewGuid(), firstName, lastName, email);
        
        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        return user;
    }
}
