
using Booking.Domain.Abstractions;

namespace Booking.Domain.Bookings.Event;

public sealed record BookingConfirmedDomainEvent(Guid Id) : IDomainEvent;
