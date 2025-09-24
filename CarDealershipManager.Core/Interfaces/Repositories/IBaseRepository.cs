namespace CarDealershipManager.Core.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllActiveAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task UpdateByIdAsync(int id, Action<T> updateAction);
        Task DeleteByIdAsync(int id);
    }
}
