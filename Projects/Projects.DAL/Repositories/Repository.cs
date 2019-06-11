using Projects.DAL.EF;
using Projects.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Projects.DAL.Repositories
{
    class Repository<T> : IRepository<T> where T : class
    {
        private ProjectContext _projectContext;
        private DbSet<T> _dbSet;

        public Repository(ProjectContext projectContext)
        {
            _projectContext = projectContext;
            _dbSet = projectContext.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet;
        }

        public T Get(Guid? id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<T> Find(Func<T, Boolean> predicate)
        {
            return GetAll().Where(predicate);
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            var entityEntry = _projectContext.Entry(entity);
            if (entityEntry.State == EntityState.Detached)
                _dbSet.Attach(entity);
            entityEntry.State = EntityState.Modified;
        }

        public void Delete(Guid id)
        {
            T entity = _dbSet.Find(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }
    }
}
