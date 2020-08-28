using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Siterm.Domain.Models;
using Siterm.Domain.Services;

namespace Siterm.EntityFramework.Services
{
    public class GenericDataService<T> : IDataService<T> where T : DomainObject
    {
        protected readonly SitermDbContextFactory ContextFactory;

        public GenericDataService(SitermDbContextFactory contextFactory)
        {
            ContextFactory = contextFactory;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            await using var context = ContextFactory.CreateDbContext();

            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> Get(int id)
        {
            await using var context = ContextFactory.CreateDbContext();

            return await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<T> Create(T entity)
        {
            await using var context = ContextFactory.CreateDbContext();

            var createdEntry = await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();

            return createdEntry.Entity;
        }

        public async Task CreateAll(IEnumerable<T> entities)
        {
            await using var context = ContextFactory.CreateDbContext();

            await context.Set<T>().AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }

        public async Task<T> Update(int id, T entity)
        {
            await using var context = ContextFactory.CreateDbContext();

            entity.Id = id;
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            await using var context = ContextFactory.CreateDbContext();

            var entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<T>> Where(Expression<Func<T, bool>> predicate)
        {
            await using var context = ContextFactory.CreateDbContext();

            return await context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            await using var context = ContextFactory.CreateDbContext();

            return await context.Set<T>().FirstOrDefaultAsync(predicate);
        }
    }
}