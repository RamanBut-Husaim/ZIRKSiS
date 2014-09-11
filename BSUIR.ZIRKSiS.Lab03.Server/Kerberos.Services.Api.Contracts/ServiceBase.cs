using Kerberos.Data.Contracts;

namespace Kerberos.Services.Api.Contracts
{
    public abstract class ServiceBase : IService
    {
        private readonly IUnitOfWork _unitOfWork;

        protected IUnitOfWork UnitOfWork
        {
            get { return this._unitOfWork; }
        }

        protected ServiceBase(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public abstract string ServiceName { get; }
    }
}
