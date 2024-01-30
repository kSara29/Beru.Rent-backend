﻿using Common;
using Deal.Dto.Booking;

namespace Deal.Application.Contracts.Booking;

public interface IBookingService
{
    Task<bool> CancelReservationAsync(Domain.Models.Booking booking);
    Task<ResponseModel<BoolResponseDto>> CreateBookingAsync(CreateBookingRequestDto dto);
    Task<List<GetBookingDatesResponse>> GetBookingDatesAsync(RequestById id);
    Task<List<BookingDto>> GetBookingsAsync(Guid id);
}