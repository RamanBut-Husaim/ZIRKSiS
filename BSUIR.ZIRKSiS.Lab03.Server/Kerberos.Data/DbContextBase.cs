using System.Data.Entity;
using Kerberos.Data.Contracts;
using Kerberos.Models;

namespace Kerberos.Data
{
    public class DbContextBase : DbContext, IDbContext
	{
		#region Constructors and Destructors

		public DbContextBase(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
			this.Configuration.LazyLoadingEnabled = false;
		}

		#endregion

		#region Public Methods and Operators

        public override int SaveChanges()
		{
			return base.SaveChanges();
		}


        public new IDbSet<T> Set<T>() where T : EntityBase
		{
			return base.Set<T>();
		}

		#endregion
	}
}