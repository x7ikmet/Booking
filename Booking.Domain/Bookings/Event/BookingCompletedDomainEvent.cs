
using Booking.Domain.Abstractions;

namespace Booking.Domain.Bookings.Event;

public sealed record BookingCompletedDomainEvent(Guid Id) : IDomainEvent;
