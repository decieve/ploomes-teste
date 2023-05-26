using ploomes_teste.persistence.Contexts;
using ploomes_teste.persistence.Contracts;

namespace ploomes_teste.persistence.Implementations
{
    public class GeneralRepository :IGeneralRepository
    {
        protected readonly PloomesContext _context;

        public GeneralRepository(PloomesContext context)
        {
            _context = context;
        }

        public virtual void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public virtual void AddRange<T>(T[] entityArray) where T:class
        {
            _context.AddRange(entityArray);
        }
        public virtual void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public virtual void DeleteRange<T>(T[] entityArray) where T : class
        {
            _context.RemoveRange(entityArray);
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public virtual void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
    }
}