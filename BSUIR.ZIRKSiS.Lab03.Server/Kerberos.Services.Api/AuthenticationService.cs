using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kerberos.Data.Contracts;
using Kerberos.Services.Api.Contracts;

namespace Kerberos.Services.Api
{
    public sealed class AuthenticationService : ServiceBase, IAuthenticationService
    {
        private readonly bool _priorAuthenticationRequired;

        private const string ServerSecretKey = "097b62aec2101ebe32abacb96fd2db622a79c6f38de5219e3db9a8e644b88cc5";

        public AuthenticationService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            this._priorAuthenticationRequired = false;
        }

        public override string ServiceName
        {
            get
            {
                return "AuthenticationService";
            }
        }


    }
}
