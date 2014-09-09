using System.ServiceModel;

namespace Kerberos.Service
{
    [ServiceContract(Namespace = "Kerberos")]
    public interface IAuthenticationService
    {
        [OperationContract]
        string GetData();
    }
}
