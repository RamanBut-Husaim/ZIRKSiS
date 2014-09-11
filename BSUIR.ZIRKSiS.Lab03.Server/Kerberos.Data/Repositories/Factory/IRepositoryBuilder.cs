using Kerberos.Data.Contracts.Repositories;
using Kerberos.Models;

namespace Kerberos.Data.Repositories.Factory
{
    internal interface IRepositoryBuilder<TEntity, TKey> where TEntity: Entity<TKey>
    {
        IRepository<TEntity, TKey> Build(UnitOfWork unitOfWork);
    }
}
