﻿using Ad.Api.DTO;
using Ad.Application.DTO;
using Ad.Application.Responses;

namespace Ad.Application.Contracts.File;

public interface IFileService
{
    Task<BaseApiResponse<string>> UploadFileAsync(CreateFileDto dto);
    Task<BaseApiResponse<string>> RemoveFileAsync(Guid id);
    Task<BaseApiResponse<byte[]>> GetFileAsync(Guid id);
}