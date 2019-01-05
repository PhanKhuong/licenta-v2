using Homefind.Core.DomainModels;
using Homefind.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homefind.Infrastructure.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly EstateDbContext _context;

        public Repository(EstateDbContext context)
        {
            _context = context;
        }

        public async Task<T> Add(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().First(x => x.Id == id);
        }

        public T GetSingleByFilter(IFilter<T> filter)
        {
            //Include subset collections in result query
            var listWithIncludedSubsets = filter.Includes
                .Aggregate(_context.Set<T>().AsQueryable(),
                    (item, subset) => item.Include(subset));

            //Apply filter criteria
            return listWithIncludedSubsets
                .Where(filter.Criteria).SingleOrDefault();
        }

        public async Task<IEnumerable<T>> ListAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> ListWithFilter(IFilter<T> filter)
        {
            //Include subset collections in result query
            var listWithIncludedSubsets = filter.Includes
                .Aggregate(_context.Set<T>().AsQueryable(),
                    (item, subset) => item.Include(subset));

            //Apply filter criteria
            return await listWithIncludedSubsets
                .Where(filter.Criteria).ToListAsync();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }
    }
}
