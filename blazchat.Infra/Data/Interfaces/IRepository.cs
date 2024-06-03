namespace blazchat.Infra.Data.Interfaces
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(Guid id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task Delete(Guid id);
    }
}