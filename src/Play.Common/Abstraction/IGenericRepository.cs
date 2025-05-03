using System.Linq.Expressions;

namespace Play.Common.Abstraction
{
    public interface IGenericRepository<T> where T: IBaseEntity
    {
        public Task<IEnumerable<T>> GetAllAsync();

        public Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        public Task<T> GetAsync(Guid id);
        public Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        public Task<T> AddAsync(T entity);
        public Task<T> UpdateAsync(T entity);
        public Task<T> DeleteAsync(Guid id);
    }
}
