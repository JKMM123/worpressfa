using Microsoft.EntityFrameworkCore;
using SmeKpiDashboard.Data;
using SmeKpiDashboard.Models;

namespace SmeKpiDashboard.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _context;

    public SaleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Sale>> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Sales
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.Date)
            .ToListAsync();
    }

    public async Task<Sale?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _context.Sales
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
    }

    public async Task<Sale> CreateAsync(Sale sale)
    {
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();
        return sale;
    }

    public async Task<Sale> UpdateAsync(Sale sale)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync();
        return sale;
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var sale = await GetByIdAsync(id, userId);
        if (sale != null)
        {
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
        }
    }
}
