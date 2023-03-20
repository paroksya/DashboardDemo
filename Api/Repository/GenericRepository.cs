using DemoDynamicRolePolicy.DBContext;
using DemoDynamicRolePolicy.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace DemoDynamicRolePolicy.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        internal readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public T GetSingle(Expression<Func<T, bool>> predicate, bool asNoTracking = false)
        {
            if (asNoTracking)
                return _context.Set<T>().AsNoTracking().FirstOrDefault(predicate);
            else
                return _context.Set<T>().FirstOrDefault(predicate);
        }
    }
}
