namespace CarDealershipManager.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllActiveAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task UpdateByIdAsync(int id);
        Task DeleteByIdAsync(int id);
    }
}
