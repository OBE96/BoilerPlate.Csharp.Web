using BoilerPlate.Domain.Entities;
using System.Linq.Expressions;


namespace BoilerPlate.Infrastructure.Repository.Interface
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> GetAsync (Guid id);

        //for example , Expression<Func<T, bool>> predicate,if the predicate is something like x.Age > 18 , which means you want to fecth persons older than 18
        //params Expression<Func<T, object>>[] includeProperties: Example: If T is a Person entity and includeProperties is used,
        //it might include related entities like x => x.Address or x => x.Orders.
        //This would instruct the method to also fetch the Address and Orders properties along with the Person.
        Task<T> GetBySpec(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        //When you see Task<IEnumerable<T>>, it means that the method returns an asynchronous task that, once completed,
        //will provide an enumerable collection of type T.
        //The method does some work asynchronously(such as fetching data from a database or calling an external service).
        //Once the operation is complete, it returns an IEnumerable<T>, which is a collection of objects of type T.
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task<IEnumerable<T>> GetAllBySpec(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> GetQueryableBySpec(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<T> DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task<bool> Exists(Guid id);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task SaveChanges();
    }
}
