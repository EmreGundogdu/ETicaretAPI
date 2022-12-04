﻿using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ETicaretAPI.Persistence.Repositories
{
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly ETicaretAPIDbContext context;

        public ReadRepository(ETicaretAPIDbContext context)
        {
            this.context = context;
        }

        public DbSet<T> Table => context.Set<T>();

        public IQueryable<T> GetAll()
        {
            return Table;
        }

        public async Task<T> GetByIdAsync(string id)
        {
            //Marker Pattern - firstordefaultasync metodunda string id'yi bulmak için baseEntity'den gelen (x.ID) ile yakaladık
            return await Table.FindAsync(Guid.Parse(id));
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression)
        {
            return await Table.FirstOrDefaultAsync(expression);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> expression)
        {
            return Table.Where(expression);
        }
    }
}
