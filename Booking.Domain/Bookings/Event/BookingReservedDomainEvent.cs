

using Booking.Domain.Abstractions;

namespace Booking.Domain.Bookings.Event;

public sealed record BookingReservedDomainEvent(Guid BookingId) : IDomainEvent;
