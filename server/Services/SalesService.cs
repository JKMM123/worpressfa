using SmeKpiDashboard.DTOs;
using SmeKpiDashboard.Models;
using SmeKpiDashboard.Repositories;

namespace SmeKpiDashboard.Services;

public class SalesService : ISalesService
{
    private readonly ISaleRepository _repository;

    public SalesService(ISaleRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SaleResponse>> GetAllAsync(Guid userId)
    {
        var sales = await _repository.GetAllByUserIdAsync(userId);
        return sales.Select(MapToResponse).ToList();
    }

    public async Task<SaleResponse?> GetByIdAsync(Guid id, Guid userId)
    {
        var sale = await _repository.GetByIdAsync(id, userId);
        return sale == null ? null : MapToResponse(sale);
    }

    public async Task<SaleResponse> CreateAsync(SaleRequest request, Guid userId)
    {
        if (request.Date.ToUniversalTime() > DateTime.UtcNow)
            throw new ArgumentException("Date cannot be in the future");

        var sale = new Sale
        {
            UserId = userId,
            Amount = request.Amount,
            Date = request.Date.ToUniversalTime(),
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(sale);
        return MapToResponse(created);
    }

    public async Task<SaleResponse> UpdateAsync(Guid id, SaleRequest request, Guid userId)
    {
        var existing = await _repository.GetByIdAsync(id, userId)
            ?? throw new KeyNotFoundException("Sale not found");

        if (request.Date.ToUniversalTime() > DateTime.UtcNow)
            throw new ArgumentException("Date cannot be in the future");

        existing.Amount = request.Amount;
        existing.Date = request.Date.ToUniversalTime();
        existing.Description = request.Description;

        var updated = await _repository.UpdateAsync(existing);
        return MapToResponse(updated);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        await _repository.DeleteAsync(id, userId);
    }

    private static SaleResponse MapToResponse(Sale sale) => new()
    {
        Id = sale.Id,
        Amount = sale.Amount,
        Date = sale.Date,
        Description = sale.Description,
        CreatedAt = sale.CreatedAt
    };
}
