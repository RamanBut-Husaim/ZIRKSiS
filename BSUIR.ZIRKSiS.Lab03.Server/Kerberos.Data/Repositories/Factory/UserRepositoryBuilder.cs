using Kerberos.Data.Contracts.Repositories;
using Kerberos.Models;

namespace Kerberos.Data.Repositories.Factory
{
    internal sealed class UserRepositoryBuilder : IRepositoryBuilder<User, int>
    {
        #region Implementation of IRepositoryBuilder<User,int>

        public IRepository<User, int> Build(UnitOfWork unitOfWork)
        {
            return new Repository<User, int>(unitOfWork.Context);
        }

        #endregion
    }
}
