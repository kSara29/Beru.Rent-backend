using Ad.Application.DTO.GetDtos;
using Ad.Application.Responses;
using Ad.Domain.Models;
using Ad.Dto;
using Ad.Dto.GetDtos;
using CreateAdDto = Ad.Dto.CreateDtos.CreateAdDto;

namespace Ad.Application.Contracts.Ad;

public interface IAdService
{
    Task<BaseApiResponse<Guid>> CreateAdAsync(CreateAdDto ad);
    Task<BaseApiResponse<AdDto>> GetAdAsync(Guid id);
    Task<BaseApiResponse<GetMainPageDto<AdMainPageDto>>> GetAllAdAsync(int page, string sortdate, string sortprice, string cat);
    Task<decimal> GetCostAsync(Guid adId, DateTime ebeg, DateTime dend);
    Task<string> GetOwnerIdAsync(Guid adId);    
    
}