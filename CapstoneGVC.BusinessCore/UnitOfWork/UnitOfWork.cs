using CapstoneGVC.Contracts.DomainServices;
using System;

namespace CapstoneGVC.BusinessCore.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IEnmascarador Enmascarador { get; }

        public IEncriptador Encriptador { get; }

        public UnitOfWork(IEnmascarador enmascarador, IEncriptador encriptador)
        {
            Enmascarador = enmascarador;
            Encriptador = encriptador;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //Release resources, no resources to release
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
