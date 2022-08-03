using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace UnionTask_Interface.IDataService
{
  public  interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void AddAll(List<T> entity);
        void Delete(T entity);
        void DeleteAll(List<T> entity);
        void Edit(T entity);
        void Save();

    }
}
