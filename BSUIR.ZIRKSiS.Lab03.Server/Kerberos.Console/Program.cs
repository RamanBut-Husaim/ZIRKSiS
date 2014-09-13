using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kerberos.Crypto;
using Kerberos.Crypto.Contracts;
using Kerberos.Data;
using Kerberos.Data.Contracts;
using Kerberos.Data.Contracts.Repositories.Factory;
using Kerberos.Data.Repositories.Factory;
using Kerberos.Models;
using Kerberos.Services.Api.Authentication;
using Kerberos.Services.Api.Contracts;
using Kerberos.Services.Api.Contracts.Authentication;
using Kerberos.Services.Api.Contracts.Tracing;
using Kerberos.Services.Api.Tracing;

namespace Kerberos.Console
{
    public sealed class Program
    {
        private static ITraceManager traceManager;

        static void Main(string[] args)
        {
            traceManager = new TraceManager("trace.txt");

            IRepositoryFactory repositoryFactory = RepositoryFactory.Instance;
            ISymmetricAlgorithmProvider symmetricAlgorithmProvider = SymmetricAlgorithmProvider.Instance;

            using (var dbContext = new KerberosStorageContext())
            {
                using (var unitOfWork = new UnitOfWork(dbContext, repositoryFactory))
                {
                    var users = unitOfWork.Repository<User, int>()
                              .Query()
                              .Filter(p => p.Email.Equals("halford@gmail.com", StringComparison.OrdinalIgnoreCase))
                              .Get();

                    var user = users.FirstOrDefault();

                    if (user != null)
                    {
                        IAuthenticationService authenticationService = new AuthenticationService(unitOfWork, symmetricAlgorithmProvider, traceManager);

                        var authenticationRequest = new AuthenticationRequest { ServerId = "authentication server", TimeStamp = DateTime.Now, UserId = user.Email };
                        traceManager.TraceAuthRequest("Authentication Request Sent", authenticationRequest);
                        IAuthenticationReply authenticationResponse = authenticationService.Authenticate(authenticationRequest);
                        traceManager.TraceTgsAuthenticationReply("TGS encrypted received", authenticationResponse.TgsBytes, authenticationResponse.TgtBytes);

                        ITgsToken tgsToken = authenticationService.DecryptReply(user.Email, authenticationResponse);
                        traceManager.TraceTgsToken("TGS decrypted: ", tgsToken);
                    }
                }
            }

            traceManager.Dispose();
        }
    }
}
