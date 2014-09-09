using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Kerberos.Service
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        public string GetData()
        {
            return "Hello World";
        }
    }
}
