
using Booking.Application.Abstractions.Messaging;

namespace Booking.Application.Bookings;

public record ReserveBookingCommand(
    Guid ApartmentId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate) : ICommand<Guid>;
