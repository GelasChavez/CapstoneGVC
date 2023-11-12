namespace CapstoneGVC.Contracts.DataTransferObjects
{
    public class EncriptadorDto
    {
        public string Hash { get; set; }
        public byte[] CadenaEncriptada { get; set; }
        public string CadenaEncriptadaString { get; set; }
        public string CadenaSinEncriptar { get; set; }
    }
}
