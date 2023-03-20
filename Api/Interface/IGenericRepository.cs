using System.Linq.Expressions;

namespace DemoDynamicRolePolicy.Interface
{
    public interface IGenericRepository<T> where T : class, new()
    {
        T GetSingle(Expression<Func<T, bool>> predicate, bool asNoTracking = false);
    }
}
