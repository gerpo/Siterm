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
        private readonly SitermDbContextFactory _contextFactory;

        public GenericDataService(SitermDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            await using var context = _contextFactory.CreateDbContext();

            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> Get(int id)
        {
            await using var context = _contextFactory.CreateDbContext();

            return await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<T> Create(T entity)
        {
            await using var context = _contextFactory.CreateDbContext();

            var createdEntry = await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();

            return createdEntry.Entity;
        }

        public async void CreateAll(IEnumerable<T> entities)
        {
            await using var context = _contextFactory.CreateDbContext();

            await context.Set<T>().AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }

        public async Task<T> Update(int id, T entity)
        {
            await using var context = _contextFactory.CreateDbContext();

            entity.Id = id;
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> Delete(int id)
        {
            await using var context = _contextFactory.CreateDbContext();

            var entity = await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<T>> Where(Expression<Func<T, bool>> predicate)
        {
            await using var context = _contextFactory.CreateDbContext();

            return await context.Set<T>().Where(predicate).ToListAsync();
        }
    }
}