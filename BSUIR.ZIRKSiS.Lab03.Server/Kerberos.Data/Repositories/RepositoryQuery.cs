using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Kerberos.Data.Contracts.Repositories;
using Kerberos.Models;

namespace Kerberos.Data.Repositories
{
    public sealed class RepositoryQuery<TEntity, TKey> : IRepositoryQuery<TEntity, TKey>
		where TEntity : Entity<TKey>
	{
		#region Fields

		private readonly List<Expression<Func<TEntity, object>>> _includedProperies;
		private readonly IRepository<TEntity, TKey> _repository;
		private Expression<Func<TEntity, bool>> _filter;
		private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> _orderedByQuerable;

		#endregion

		#region Constructors and Destructors

		public RepositoryQuery(IRepository<TEntity, TKey> repository)
		{
			this._repository = repository;
			this._includedProperies = new List<Expression<Func<TEntity, object>>>();
		}

		#endregion

		#region Public Methods and Operators

		public IRepositoryQuery<TEntity, TKey> Filter(Expression<Func<TEntity, bool>> filter)
		{
			this._filter = filter;
			return this;
		}

		public IEnumerable<TEntity> Get()
		{
			return ((Repository<TEntity, TKey>)this._repository).Get(
				this._filter,
				this._orderedByQuerable,
				this._includedProperies);
		}

		public IRepositoryQuery<TEntity, TKey> Include(Expression<Func<TEntity, object>> expression)
		{
			this._includedProperies.Add(expression);
			return this;
		}

		public IRepositoryQuery<TEntity, TKey> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
		{
			this._orderedByQuerable = orderBy;
			return this;
		}

		#endregion
	}
}