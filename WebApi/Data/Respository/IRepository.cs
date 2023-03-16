namespace WebApi.Data.Respository
{
    public interface IRepository<T> : IDisposable where T : class 
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(Guid id);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(Guid id);
        Task SaveAsync();
    }
}
