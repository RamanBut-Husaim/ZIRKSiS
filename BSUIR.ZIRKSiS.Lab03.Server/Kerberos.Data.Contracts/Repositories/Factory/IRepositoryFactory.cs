using Kerberos.Models;

namespace Kerberos.Data.Contracts.Repositories.Factory
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity, TKey> BuildRepository<TEntity, TKey>(IUnitOfWork unitOfWork) 
            where TEntity : Entity<TKey>;
    }
}
