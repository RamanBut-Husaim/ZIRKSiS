using System.Data.Entity;
using Kerberos.Data.Initialization;
using Kerberos.Data.Mapping;

namespace Kerberos.Data
{
    public sealed class KerberosStorageContext : DbContextBase
	{
		static KerberosStorageContext()
		{
		    Database.SetInitializer(new InitializeKerberosStorageIfModelChanges());
		}

        public KerberosStorageContext()
			: base("Kerberos")
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
		    modelBuilder.Configurations.Add(new UserMap());
		}
	}
}