using System;
using System.Collections.Generic;

namespace Projects.DAL.Interfaces
{
    public interface IPartialRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(Guid? id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
    }
}
