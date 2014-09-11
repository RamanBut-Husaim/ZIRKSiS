using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Kerberos.Models;

namespace Kerberos.Data.Contracts
{
    public interface IDbContext
    {
        #region Public Methods and Operators

        void Dispose();

        DbEntityEntry Entry(object o);

        int SaveChanges();

        IDbSet<T> Set<T>() where T : EntityBase;

        #endregion
    }
}