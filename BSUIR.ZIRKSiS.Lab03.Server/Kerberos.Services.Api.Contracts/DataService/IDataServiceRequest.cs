namespace Kerberos.Services.Api.Contracts.DataService
{
    public interface IDataServiceRequest
    {
        byte[] ServiceTicket { get; set; }
        byte[] Authenticator { get; set; }
    }
}
