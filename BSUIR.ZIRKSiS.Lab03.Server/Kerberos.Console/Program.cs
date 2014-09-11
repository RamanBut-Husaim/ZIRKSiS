using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kerberos.Data;
using Kerberos.Data.Contracts;
using Kerberos.Data.Contracts.Repositories.Factory;
using Kerberos.Data.Repositories.Factory;
using Kerberos.Models;

namespace Kerberos.Console
{
    public sealed class Program
    {
        static void Main(string[] args)
        {
            IRepositoryFactory repositoryFactory = RepositoryFactory.Instance;
            using (var dbContext = new KerberosStorageContext())
            {
                using (var unitOfWork = new UnitOfWork(dbContext, repositoryFactory))
                {
                    var users = unitOfWork.Repository<User, int>().Query().Filter(p => p.Key > 0).Get();
                }
            }
        }
    }
}
