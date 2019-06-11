using System;
using System.Collections.Generic;

namespace Projects.BLL.Interfaces
{
    public interface IService<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(Guid id);
        void Add(T item);
        void Update(T item);
        void Delete(Guid id);
        void Dispose();
    }
}
