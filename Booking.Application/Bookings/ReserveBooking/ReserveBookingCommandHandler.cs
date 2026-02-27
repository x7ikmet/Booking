using Booking.Application.Abstractions.Clock;
using Booking.Application.Abstractions.Messaging;
using Booking.Domain.Abstractions;
using Booking.Domain.Apartments;
using Booking.Domain.Bookings;
using Booking.Domain.Users;

namespace Booking.Application.Bookings.ReserveBooking;

internal sealed class ReserveBookingCommandHandler : ICommandHandler<ReserveBookingCommand, Guid>
{
    private readonly IUserReposotory _userReposotory;
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PricingService _pricingService;
    private readonly IDateTimeProvider _dateProvider;

    public ReserveBookingCommandHandler(
        IUserReposotory userReposotory, 
        IApartmentRepository apartmentRepository, 
        IBookingRepository bookingRepository, 
        IUnitOfWork unitOfWork, 
        PricingService pricingService,
        IDateTimeProvider dateTimeProvider)
    {
        _userReposotory = userReposotory;
        _apartmentRepository = apartmentRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _pricingService = pricingService;
        _dateProvider = dateTimeProvider;
    }

    public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
    {
        var user = await _userReposotory.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        var apartment = await _apartmentRepository.GetByIdAsync(request.ApartmentId, cancellationToken);
        if (apartment == null)
        {
            return Result.Failure<Guid>(ApartmentErrors.NotFound);
        }

        var duration = DateRange.Create(request.StartDate, request.EndDate);
        if (await _bookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken))
        {
            return Result.Failure<Guid>(BookingErrors.Overlap);
        }

        var booking = Domain.Bookings.Booking.Reserve(
            apartment,
            user.Id,
            duration,
            _dateProvider.UtcNow,
            _pricingService);
        _bookingRepository.Add(booking);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return booking.Id;
    }
}
