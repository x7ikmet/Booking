
using Booking.Domain.Abstractions;

namespace Booking.Domain.Bookings.Event;

public sealed record BookingRejectedDomainEvent(Guid Id) : IDomainEvent;
