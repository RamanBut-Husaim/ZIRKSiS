using System;
using System.Collections;
using Kerberos.Data.Contracts;
using Kerberos.Data.Contracts.Repositories;
using Kerberos.Data.Contracts.Repositories.Factory;
using Kerberos.Models;

namespace Kerberos.Data
{
    public class UnitOfWork : IUnitOfWork
	{
		#region Fields

		private readonly IDbContext _context;
	    private readonly IRepositoryFactory _repositoryFactory;

		private readonly Hashtable _repositories;
		private bool _isDisposed;

		#endregion

		#region Constructors and Destructors

		public UnitOfWork(IDbContext context, IRepositoryFactory repositoryFactory)
		{
			this._context = context;
			this._repositories = new Hashtable();
		    this._repositoryFactory = repositoryFactory;
		}

		#endregion

	    internal IDbContext Context
	    {
            get { return this._context; }
	    }

		#region Public Methods and Operators

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual void Dispose(bool disposing)
		{
			if (!this._isDisposed)
			{
				if (disposing)
				{
					this._context.Dispose();
				}
			}

			this._isDisposed = true;
		}

		public IRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : Entity<TKey>
		{
			string type = typeof(TEntity).Name;
			if (!this._repositories.ContainsKey(type))
			{
			    object repositoryInstance = this._repositoryFactory.BuildRepository<TEntity, TKey>(this);
				this._repositories.Add(type, repositoryInstance);
			}

			return (IRepository<TEntity, TKey>)this._repositories[type];
		}

		public void Save()
		{
			this._context.SaveChanges();
		}

		#endregion
	}
}