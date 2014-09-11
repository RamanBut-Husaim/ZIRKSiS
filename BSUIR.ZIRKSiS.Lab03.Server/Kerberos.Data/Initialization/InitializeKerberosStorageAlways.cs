using System.Data.Entity;

namespace Kerberos.Data.Initialization
{
    internal sealed class InitializeKerberosStorageAlways : DropCreateDatabaseAlways<KerberosStorageContext>
    {
        protected override void Seed(KerberosStorageContext context)
        {
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
