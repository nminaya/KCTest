﻿using KCTest.Domain.Entities;
using KCTest.Domain.Repositories;
using KCTest.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KCTest.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _entities;

        public Repository(DbSet<TEntity> entities) => _entities = entities;

        public async Task AddAsync(TEntity entity) => await _entities.AddAsync(entity);
        public async Task UpdateAsync(TEntity entity) => await Task.Run(() => { _entities.Update(entity); });
        public async Task<TEntity> GetByIdAsync(int id, IEnumerable<string> entitiesToInclude) => await _entities.Include(entitiesToInclude).FirstOrDefaultAsync(e => e.Id == id);
        public async Task<IEnumerable<TEntity>> GetAllAsync(IEnumerable<Expression<Func<TEntity, bool>>> predicates, IEnumerable<string> entitiesToInclude) => await _entities.Filter(predicates).Include(entitiesToInclude).ToListAsync();
        public void Delete(TEntity entity) => _entities.Remove(entity);
        public async Task DeleteAsync(TEntity entity) => await Task.Run(() => { Delete(entity); });
        public async virtual Task<int> Count(IEnumerable<Expression<Func<TEntity, bool>>> predicates = null) => await _entities.Filter(predicates).CountAsync();
        public async Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate) => await _entities.AnyAsync(predicate);
    }
}