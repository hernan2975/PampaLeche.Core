using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PampaLeche.Domain.Entities;
using PampaLeche.Domain.Interfaces;

namespace PampaLeche.Infrastructure.Persistence.Repositories;

public class BatchRepository : IRepository<MilkBatch>
{
    private readonly ApplicationDbContext _context;

    public BatchRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(MilkBatch entity)
    {
        _context.Batches.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<MilkBatch?> GetByIdAsync(Guid id)
    {
        return await _context.Batches.FindAsync(id);
    }
}
