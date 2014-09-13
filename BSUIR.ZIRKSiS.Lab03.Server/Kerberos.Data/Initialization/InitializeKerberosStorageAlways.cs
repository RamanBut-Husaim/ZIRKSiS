using System.Data.Entity;
using Kerberos.Crypto;
using Kerberos.Models;

namespace Kerberos.Data.Initialization
{
    internal sealed class InitializeKerberosStorageAlways : DropCreateDatabaseAlways<KerberosStorageContext>
    {
        protected override void Seed(KerberosStorageContext context)
        {
            context.SaveChanges();

            string salt = CryptoHelper.GenerateSalt(User.MaxLengthFor.PasswordSalt);
            string passwordHash = CryptoHelper.ComputePasswordHash("passwordd", salt);
            var user01 = new User("halford@gmail.com")
            {
                IsSignedIn = false,
                PasswordHash = passwordHash,
                PasswordSalt = salt
            };
            context.Set<User>().Add(user01);

            base.Seed(context);
        }
    }
}
