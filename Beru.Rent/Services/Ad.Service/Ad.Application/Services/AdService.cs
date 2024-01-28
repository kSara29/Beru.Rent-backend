using Ad.Application.Contracts.Ad;
using Ad.Application.Contracts.File;
using Ad.Application.DTO.GetDtos;
using Ad.Application.Mapper;
using Ad.Application.Contracts.Ad;
using Ad.Application.Responses;
using Ad.Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Ad.Dto;
using Ad.Dto.GetDtos;
using Ad.Dto.ResponseDto;
using Common;
using Microsoft.AspNetCore.Http;
using CreateAdDto = Ad.Dto.CreateDtos.CreateAdDto;

namespace Ad.Application.Services;

public class AdService : IAdService
{
    private readonly IAdRepository _repository;
    private readonly IFileRepository _fileRepository;

    public AdService(IAdRepository repository,IFileRepository fileRepository)
    {
        _repository = repository;
        _fileRepository = fileRepository;
    }
    public async Task<ResponseModel<Guid>> CreateAdAsync(CreateAdDto ad)
    {
       var result =await _repository.CreateAdAsync(ad.ToDomain());
       return ResponseModel<Guid>.CreateSuccess(result);
    }

    public async Task<ResponseModel<AdDto>> GetAdAsync(Guid id)
    {
        var result = await _repository.GetAdAsync(id);
        if (result != null)
        {
              var data = result.ToDto();
              var files = await _fileRepository.GetAllFilesAsync(id);
              data.Files = files;
              var response = ResponseModel<AdDto>.CreateSuccess(data);
        }

        var errors = new List<ResponseError>();
        var errorModel = new ResponseError("404", "С таким Id объявления не найдено");
        errors.Add(errorModel);

        return ResponseModel<AdDto>.CreateFailed(errors);
    }
    

    public async Task<ResponseModel<GetMainPageDto<AdMainPageDto>>> GetAllAdAsync(int page, string sortdate, string sortprice, string cat)
    {
        var result = await _repository.GetAllAdAsync(page,sortdate,sortprice,cat);
        var mainPageDto = new GetMainPageDto<AdMainPageDto>(result.MainPageDto.Select(ad => ad.ToMainPageDto()).ToList(), result.TotalPage);
        
        foreach (var dto in mainPageDto.MainPageDto)
        {
            var files = await _fileRepository.GetAllFilesAsync(dto.Id);
            dto.Files = files;
        }

        return ResponseModel<GetMainPageDto<AdMainPageDto>>.CreateSuccess(mainPageDto);
    }

    public async Task<ResponseModel<decimal>> GetCostAsync(Guid adId, DateTime dbeg, DateTime dend)
    {
        var result =await _repository.GetCostAsync(adId, dbeg,dend);
        return ResponseModel<decimal>.CreateSuccess(result);
    }

    public async Task<ResponseModel<string>> GetOwnerIdAsync(Guid adId)
    {
        var result =await _repository.GetOwnerIdAsync(adId);
        
        return ResponseModel<string>.CreateSuccess(result);
    }
}