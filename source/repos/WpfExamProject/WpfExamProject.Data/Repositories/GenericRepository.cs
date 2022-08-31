using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WpfExamProject.Data.Contexts;
using WpfExamProject.Data.IRepositories;

namespace WpfExamProject.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        public ExamProjectDBContext dbContext;
        protected readonly DbSet<T> dbSet;

        public GenericRepository()
        {
            dbContext = new ExamProjectDBContext();
            dbSet = dbContext.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
        {
            var response = (await dbSet.AddAsync(entity)).Entity;

            await dbContext.SaveChangesAsync();

            return response;

        }
        public IQueryable<T> GetAll(Expression<Func<T, bool>> expression = null)
            => expression is null ? dbSet : dbSet.Where(expression);

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression)
            => await dbSet.FirstOrDefaultAsync(expression);

        public T Update(T entity)
        {
            var res = dbSet.Update(entity).Entity;

            dbContext.SaveChanges();

            return res;

        }
        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> expression)
        {
            var entity = await dbSet.FirstOrDefaultAsync(expression);

            if (entity is null)
                return false;

            dbSet.Remove(entity);

            dbContext.SaveChanges();

            return true;
        }


    }
}
