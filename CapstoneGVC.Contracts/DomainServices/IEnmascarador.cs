using CapstoneGVC.Contracts.DataTransferObjects;

namespace CapstoneGVC.Contracts.DomainServices
{
    public interface IEnmascarador
    {
        MascaraDto EnmascaraNumeroTarjeta(TarjetaDto tarjeta);
    }
}
