using FinalProjectFb.Domain.Entities;
using FinalProjectFb.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.Abstractions.Repositories.Generic
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        IQueryable<T> GetAll(bool IgnoreQuery = false, bool IsTracking = false, params string[] includes);

		IQueryable<T> GetAllWhere(Expression<Func<T, bool>>? expression = null, bool isTracking = false, params string[] includes);
        IQueryable<T> GetOrder(Expression<Func<T, object>>? orderExpression = null, bool isDescending = false);
		IQueryable<T> GetPagination(int skip = 0, int take = 0, bool IgnoreQuery = false, Expression<Func<T, object>>? orderExpression = null, bool IsDescending = false, params string[] includes);
		Task<T> GetByIdAsync(int id, bool isTracking = false, bool? isDeleted = null , bool ignoreQuery = false, params string[] includes);
        Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression,  bool? isDeleted = null , bool isTracking = false, bool ignoreQuery = false, params string[] includes);
        Task<bool> IsExistAsync(Expression<Func<T, bool>> expression);
        Task<bool> IsExist(Expression<Func<T, bool>> expression);
        IQueryable<T> GetPaginationC<T>(Expression<Func<T, bool>> filter = null, int skip = 0, int take = 10) where T : class;
        IQueryable<T> GetAllnotDeleted(bool isTracking = false, params string[] includes);
        Task<T> GetByIdnotDeletedAsync(int id, bool isTracking = false, params string[] includes);

           Task AddAsync(T entity);
        void Update(T entity);
        
        void Delete(T entity);
        void SoftDelete(T entity);
        void ReverseDelete(T entity);
        Task SaveChangesAsync();
    }
}
