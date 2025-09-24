using Microsoft.EntityFrameworkCore;
using CarDealershipManager.Core.Models;
using CarDealershipManager.Infrastructure.Data;
using CarDealershipManager.Core.Interfaces.Repositories;

namespace CarDealershipManager.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task DeleteByIdAsync(int id)
        {
            var entity = _dbSet.FirstOrDefault(e => e.Id == id);
            if (entity == null) throw new InvalidOperationException("Entidade não encontrada");
            
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllActiveAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.IgnoreQueryFilters().ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateByIdAsync(int id, Action<T> updateAction)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null) throw new InvalidOperationException("Entidade não encontrada");

            updateAction(entity);
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
