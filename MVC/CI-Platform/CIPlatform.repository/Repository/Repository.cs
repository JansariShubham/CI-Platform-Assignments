using CIPlatform.entities.DataModels;
using CIPlatform.repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
/*using CIPlatform.repository.Data;*/
namespace CIPlatform.repository.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;
        internal DbSet<T> dbSet;
        public Repository(AppDbContext appDbContext) {
            _appDbContext= appDbContext;
            this.dbSet= _appDbContext.Set<T>();
        }
        public void Add(T entity)
        {
            Console.WriteLine("Addded...");
            dbSet.Add(entity);
        }

        public void Delete(T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;    
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            dbSet.RemoveRange(items);
        }
    }
}
