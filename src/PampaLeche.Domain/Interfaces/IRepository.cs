using System;
using System.Threading.Tasks;

namespace PampaLeche.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task<T?> GetByIdAsync(Guid id);
}
