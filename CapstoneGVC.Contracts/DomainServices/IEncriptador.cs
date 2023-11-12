using CapstoneGVC.Contracts.DataTransferObjects;

namespace CapstoneGVC.Contracts.DomainServices
{
    public interface IEncriptador
    {
        EncriptadorDto EncriptarCadena(EncriptadorDto cadena);

        EncriptadorDto DesencriptarCadena(EncriptadorDto cadena);

        EncriptadorDto ObtenerHash(EncriptadorDto cadena);
    }
}
