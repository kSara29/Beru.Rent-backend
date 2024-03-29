﻿using Ad.Application.Contracts.Category;
using Ad.Domain.Models;
using Ad.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Ad.Infrastructure.Database;

public class CategoryRepository:ICategoryRepository
{
    private readonly AdContext _context;

    public CategoryRepository(AdContext context)
    {
        _context = context;
    }
    public async Task<Guid> CreateAsync(Category entity)
    {
        entity.CreatedAt = DateTime.Now;
        await _context.Categories.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<string> RemoveAsync(Guid id)
    {
        var entity = await _context.Categories.FindAsync(id);
        if (entity == null)
        {
            return "Некорректный id, категория не найдена";
        }
        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return "success";
    }

    public async Task<Category?> GetAsync(Guid id)
    {
        var entity = await _context.Categories.FindAsync(id);
        return entity;
    }

    public async Task<List<Category?>?> GetAllAsync()
    {
        var result = await _context.Categories.ToListAsync();
        return result;
    }
}