﻿using Ad.Domain.Enums;
using Ad.Domain.Models;

namespace Ad.Dto.GetDtos;

public class AdDto
{
    public Guid Id { get; set; }
    public string? CreatedAt { get; set; }
    public string? UserId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ExtraConditions { get; set; }
    public bool? NeededDeposit { get; set; }
    public decimal? MinDeposit { get; set; }
    public AdState? State { get; set; }
    public decimal? Price { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? TimeUnitId { get; set; }
    public TimeUnit? TimeUnit { get; set; }
    public ContractType? ContractType { get; set; }
    public Guid? AddressExtraId { get; set; }
    public AddressExtra? AddressExtra { get; set; }
    public Category? Category { get; set; }
    public List<byte[]>? Files { get; set; }
}

