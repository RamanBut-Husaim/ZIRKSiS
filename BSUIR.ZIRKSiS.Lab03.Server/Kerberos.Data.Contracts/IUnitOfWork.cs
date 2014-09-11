using System;
using Kerberos.Data.Contracts.Repositories;
using Kerberos.Models;

namespace Kerberos.Data.Contracts
{
    public interface IUnitOfWork : IDisposable
	{
		#region Public Methods and Operators

		void Dispose(bool disposing);

		IRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : Entity<TKey>;

		void Save();

		#endregion
	}
}