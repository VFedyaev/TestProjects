using Projects.DAL.EF;
using Projects.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Projects.DAL.Repositories
{
    class PartialRepository<T> : IPartialRepository<T> where T : class
    {
        private ProjectContext _projectContext;
        private DbSet<T> _dbSet;

        public PartialRepository(ProjectContext projectContext)
        {
            _projectContext = projectContext;
            _dbSet = projectContext.Set<T>();
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet;
        }

        public T Get(Guid? id)
        {
            return _dbSet.Find(id);
        }
    }
}
