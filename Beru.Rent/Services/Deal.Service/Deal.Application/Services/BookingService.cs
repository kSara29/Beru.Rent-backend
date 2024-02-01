﻿using Common;
using Deal.Application.Contracts.Booking;
using Deal.Application.Mapper;
using Deal.Domain.Models;
using Deal.Dto.Booking;

namespace Deal.Application.Services;

public class BookingService: IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }   

    public async Task<bool> CancelReservationAsync(Booking booking)
    {
        return await (_bookingRepository.CancelReservationAsync(booking));
    }

    public async Task<ResponseModel<BoolResponseDto>> CreateBookingAsync(CreateBookingRequestDto dto)
    {
        if (dto.Dbeg < DateTime.UtcNow.AddMinutes(-1)) //Написать в сервисе + добавить ошибку и вернуть fail
        {
            var errors = new List<ResponseError>();
            var errorModel = new ResponseError()
            {
                Code = "400",
                Message = "Ввели прошедшую дату"
            };
            errors.Add(errorModel);
            return ResponseModel<BoolResponseDto>.CreateFailed(errors);
        }


        bool boolean = await _bookingRepository.CreateBookingAsync(dto);
        if (boolean)
            return ResponseModel<BoolResponseDto>.CreateSuccess(boolean.ToDto());
        else
        {
            var errors = new List<ResponseError>();
            var errorModel = new ResponseError()
            {
                Code = "400",
                Message = "Забронирванные даты недоступны"
            };
            errors.Add(errorModel);
            return ResponseModel<BoolResponseDto>.CreateFailed(errors); 
        }
        
    }

    public async Task<List<GetBookingDatesResponse>> GetBookingDatesAsync(RequestById id)
    {
        List<Booking> books = await _bookingRepository.GetBookingDatesAsync(id);
        List<GetBookingDatesResponse> list = new List<GetBookingDatesResponse>();
        foreach (var book in books) 
            list.Add(book.ToDateDto());
        return list;
    }

    public async Task<List<GetAllBookingsResponseDto>> GetAllBookingsAsync(RequestByUserId id)
    {
        var list = await _bookingRepository.GetAllBookingsAsync(id);
        List<GetAllBookingsResponseDto> result = new List<GetAllBookingsResponseDto>();
        foreach (var book in list) 
            result.Add(book.ToDtoes());
        return result;
    }

    public async Task<GetBookingResponseDto> GetBookingAsync(RequestById id)
    {
        var res = await _bookingRepository.GetBookingAsync(id);
        return res.ToDto();
    }
}