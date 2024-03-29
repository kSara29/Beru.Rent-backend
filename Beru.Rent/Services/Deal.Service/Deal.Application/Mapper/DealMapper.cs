﻿
using Deal.Dto.Booking;

namespace Deal.Application.Mapper;

public static class DealMapper
{
    public static GetDealResponseDto ToDto(this Domain.Models.Deal deal)
    {
        return new GetDealResponseDto()
        {
            Id = deal.Id,
            AdId = deal.AdId,
            TenantId = deal.TenantId,
            Dbeg = deal.Dbeg,
            Dend = deal.Dend,
            Cost = deal.Cost,
            Deposit = deal.Deposit,
            ChatId = deal.ChatId,
            OwnerId = deal.OwnerId,
            OwnerName = "NoOwnerName",
            TenantName = "NoTenantName",
            DealState = deal.DealState.ToString()
        };
    }

    public static GetAllDealsResponseDto ToDtoDeals(this Domain.Models.Deal deal)
        => new(
        deal.Id,
        deal.Dbeg,
        deal.Dend,
        deal.Cost
            );

    public static CloseDealResponseDto ToDtoForClose(this bool Bool)
        => new(
            Bool
            );
}