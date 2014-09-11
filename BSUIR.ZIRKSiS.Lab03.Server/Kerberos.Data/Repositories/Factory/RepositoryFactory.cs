using System;
using System.Collections;
using Kerberos.Data.Contracts;
using Kerberos.Data.Contracts.Repositories;
using Kerberos.Data.Contracts.Repositories.Factory;
using Kerberos.Models;

namespace Kerberos.Data.Repositories.Factory
{
    public sealed class RepositoryFactory : IRepositoryFactory
    {
        private static readonly Lazy<RepositoryFactory> Lazy = new Lazy<RepositoryFactory>(() => new RepositoryFactory(), true);

        private readonly Hashtable _repositoryBuilders;

        public static IRepositoryFactory Instance
        {
            get
            {
                return Lazy.Value;
            }
        }

        private RepositoryFactory()
        {
            this._repositoryBuilders = new Hashtable();
            this._repositoryBuilders.Add(typeof(User).Name, new UserRepositoryBuilder());
        }

        #region Implementation of IRepositoryFactory

        public IRepository<TEntity, TKey> BuildRepository<TEntity, TKey>(IUnitOfWork unitOfWork)
            where TEntity : Entity<TKey>
        {
            IRepository<TEntity, TKey> result = null;

            string entityType = typeof(TEntity).Name;
            var repositoryBuilder = this._repositoryBuilders[entityType] as IRepositoryBuilder<TEntity, TKey>;
            if (repositoryBuilder != null)
            {
                result = repositoryBuilder.Build(unitOfWork as UnitOfWork);
            }

            return result;
        }

        #endregion
    }
}
