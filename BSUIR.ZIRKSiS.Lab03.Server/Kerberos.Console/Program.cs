using System;
using System.Linq;
using Kerberos.Crypto;
using Kerberos.Crypto.Contracts;
using Kerberos.Data;
using Kerberos.Data.Contracts.Repositories.Factory;
using Kerberos.Data.Repositories.Factory;
using Kerberos.Models;
using Kerberos.Services.Api.Authentication;
using Kerberos.Services.Api.Authorization;
using Kerberos.Services.Api.Contracts;
using Kerberos.Services.Api.Contracts.Authentication;
using Kerberos.Services.Api.Contracts.Authorization;
using Kerberos.Services.Api.Contracts.DataService;
using Kerberos.Services.Api.DataService;
using Kerberos.Services.Api.Tracing;

namespace Kerberos.Console
{
    public sealed class Program
    {
        static void Main(string[] args)
        {
            IRepositoryFactory repositoryFactory = RepositoryFactory.Instance;
            ISymmetricAlgorithmProvider symmetricAlgorithmProvider = SymmetricAlgorithmProvider.Instance;

            using (var traceManager = new TraceManager("trace.txt"))
            {
                using (var dbContext = new KerberosStorageContext())
                {
                    using (var unitOfWork = new UnitOfWork(dbContext, repositoryFactory))
                    {
                        var users =
                            unitOfWork.Repository<User, int>()
                                      .Query()
                                      .Filter(p => p.Email.Equals("halford@gmail.com", StringComparison.OrdinalIgnoreCase))
                                      .Get();

                        var user = users.FirstOrDefault();

                        if (user != null)
                        {
                            IAuthenticationService authenticationService = new AuthenticationService(
                                unitOfWork,
                                symmetricAlgorithmProvider,
                                traceManager);

                            var authenticationRequest = new AuthenticationRequest
                            {
                                ServerId = "authentication server",
                                TimeStamp = DateTime.Now,
                                UserId = user.Email
                            };
                            traceManager.Trace("Authentication Request Sent", authenticationRequest);
                            IAuthenticationReply authenticationReply = authenticationService.Authenticate(authenticationRequest);
                            traceManager.Trace("TGS encrypted received", Tuple.Create(authenticationReply.TgsBytes, authenticationReply.TgtBytes));

                            ITgsToken tgsToken = authenticationService.DecryptReply(user.Email, authenticationReply);
                            traceManager.Trace("TGS decrypted: ", tgsToken);

                            IAuthorizationService authorizationService = new AuthorizationService(
                                unitOfWork,
                                symmetricAlgorithmProvider,
                                traceManager);
                            byte[] authenticator = authorizationService.CreateAuthenticator(user.Email, tgsToken.SessionKey);
                            traceManager.Trace("Auth authenticator encrypted", authenticator);

                            var authorizationRequest = new AuthorizationRequest
                            {
                                AutheticatorBytes = authenticator,
                                TgtBytes = authenticationReply.TgtBytes
                            };

                            IAuthorizationReply authorizationReply = authorizationService.Authorize(authorizationRequest);
                            traceManager.Trace("Authorization reply received", Tuple.Create(authorizationReply.ServiceTicket, authorizationReply.ServiceToken));

                            IServiceToken serviceToken = authorizationService.DecryptReply(user.Email, authorizationReply, tgsToken.SessionKey);
                            traceManager.Trace("Service token decrypted", serviceToken);

                            IDataService dataService = new DataService(unitOfWork, symmetricAlgorithmProvider, traceManager);
                            authenticator = dataService.CreateAuthenticator(user.Email, serviceToken.SessionKey);

                            var dataServiceRequest = new DataServiceRequest
                            {
                                Authenticator = authenticator,
                                ServiceTicket = authorizationReply.ServiceTicket
                            };

                            traceManager.Trace("Data service request sent", dataServiceRequest);

                            IDataServiceReply dataServiceReply = dataService.GetAccess(dataServiceRequest);
                            ITimeStampContainer timeStampContainer = dataService.DecryptReply(user.Email, dataServiceReply, serviceToken.SessionKey);

                            if (Math.Abs((DateTime.UtcNow - new DateTime(timeStampContainer.TimeStamp, DateTimeKind.Utc)).Ticks) < new TimeSpan(1, 0, 0).Ticks)
                            {
                                traceManager.Trace("The operation has been completed successfully.");
                            }
                        }
                    }
                }

            }
        }
    }
}
