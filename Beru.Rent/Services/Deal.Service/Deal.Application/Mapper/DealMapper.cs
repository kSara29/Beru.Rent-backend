﻿
using Deal.Dto.Booking;

namespace Deal.Application.Mapper;

public static class DealMapper
{
    public static GetDealResponseDto ToDto(this Domain.Models.Deal deal)
        => new(
            deal.AdId,
            deal.AdId,
            deal.TenantId,
            deal.Dbeg,
            deal.Dend,
            deal.Cost,
            deal.Deposit,
            deal.ChatId
            );

    public static GetAllDealsResponseDto ToDtoDeals(this Domain.Models.Deal deal)
        => new(
        deal.Id,
        deal.Dbeg,
        deal.Dend,
        deal.Cost
            );
}