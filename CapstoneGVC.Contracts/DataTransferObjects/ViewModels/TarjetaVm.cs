namespace CapstoneGVC.Contracts.DataTransferObjects.ViewModels
{
    public class TarjetaVm
    {
        //ALMACENA LA TARJETA CAPTURADA POR EL USUARIO
        public TarjetaDto Tarjeta { get; set; }
        //ALMACENA LA TARJETA CON LA MASCARA APLICADA
        public MascaraDto Mascara { get; set; }
        //ALMACENA EL HASH DE LA TARJETA CAPTURADA
        public EncriptadorDto TarjetaConHash { get; set; }
        //ALMACENA LA CADENA  ENCRIPTADA
        public EncriptadorDto TarjetaEncriptada { get; set; }
        //ALMACENA LA CADENA DESENCRIPTADA
        public EncriptadorDto TarjetaDesencriptada { get; set; }
        //ALMACENA EL HASH DE LA TARJETA DESENCRIPTADA
        public EncriptadorDto HashDeTarjetaDesencriptada { get; set; }
        //ALMACENA EL RESULTADO AL COMPARAR LOS DOS HASH
        public bool HashsCoinciden { get; set; }
    }
}
