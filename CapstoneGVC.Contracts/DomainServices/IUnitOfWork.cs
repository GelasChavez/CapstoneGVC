using System;

namespace CapstoneGVC.Contracts.DomainServices
{
    public interface IUnitOfWork : IDisposable
    {
        IEnmascarador Enmascarador { get; }
        IEncriptador Encriptador { get; }
    }
}
