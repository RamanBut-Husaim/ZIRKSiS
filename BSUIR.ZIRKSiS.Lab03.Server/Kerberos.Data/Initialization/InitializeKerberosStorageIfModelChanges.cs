using System.Data.Entity;

namespace Kerberos.Data.Initialization
{
    internal sealed class InitializeKerberosStorageIfModelChanges : DropCreateDatabaseIfModelChanges<KerberosStorageContext>
    {
        protected override void Seed(KerberosStorageContext context)
        {
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
