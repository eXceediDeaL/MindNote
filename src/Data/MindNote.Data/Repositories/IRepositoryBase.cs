using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MindNote.Data.Repositories
{
    public interface IRepositoryBase<T, TId, TMutation> where T : IHasId<TId>
    {
        Task<T> Get(TId id, string identity);

        Task<TId> Create(T data, string identity);

        Task<TId> Update(TId id, TMutation mutation, string identity);

        Task<TId> Delete(TId id, string identity);

        Task<bool> Clear(string identity);

        Task<IQueryable<T>> Query(string identity, Expression<Func<T, bool>> condition = null);
    }
}
