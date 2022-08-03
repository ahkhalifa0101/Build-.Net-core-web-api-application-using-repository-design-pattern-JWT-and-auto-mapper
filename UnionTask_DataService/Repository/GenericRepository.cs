using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnionTask_Context.DBContext;
using UnionTask_Interface.IDataService;

namespace UnionTask_DataService.Repository
{
  public abstract class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        protected UnionContext Context;

        public GenericRepository(UnionContext c)
        {
            Context = c;
        }
        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = Context.Set<T>();
            return query;
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            var query = Context.Set<T>().Where(predicate);
            return query;
        }
 

        public virtual void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }
        public virtual void AddAll(List<T> entity)
        {
            Context.Set<T>().AddRange(entity);
            Context.SaveChanges();
        }
        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }
        public virtual void DeleteAll(List<T> entity)
        {
            Context.Set<T>().RemoveRange(entity);
            Context.SaveChanges();
        }

        public virtual void Edit(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }


        public virtual void Save()
        {
            Context.SaveChanges();
        }

    }
}
