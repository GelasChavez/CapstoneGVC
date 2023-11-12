using CapstoneGVC.Contracts.DataTransferObjects;
using CapstoneGVC.Contracts.DomainServices;
using CapstoneGVC.Contracts.Enums;
using System.Text;

namespace CapstoneGVC.BusinessCore.Domain
{
    public class Enmascarador : IEnmascarador
    {
        /// <summary>
        /// Metodo que aplica una mascara de * al numero de tarjeta
        /// </summary>
        /// <param name="tarjeta">Objeto TarjetaDto que almacena el numero de tartjeta a enmascarar</param>
        /// <returns>Regresa un ojeto MascaraDto en donde se almacena el numero de la tarjeta con una mascara</returns>
        public MascaraDto EnmascaraNumeroTarjeta(TarjetaDto tarjeta)
        {
            int cantidadDigitosMostrar = (int)ConfiguracionTarjeta.CANTIDAD_DIGITOS_A_MOSTRAR;
            var longitudTarjeta = tarjeta.NumeroTarjeta.Length;
            var cantidadValoresEncrtiptar = longitudTarjeta - cantidadDigitosMostrar;

            int contador = 1;
            StringBuilder mascara = new StringBuilder("", longitudTarjeta);

            while ((int)ConfiguracionTarjeta.DIGITOS_PERMITIDOS >= contador)
            {
                if (contador <= cantidadValoresEncrtiptar)
                {
                    mascara.Append("*");
                }
                else
                {
                    mascara.Append(tarjeta.NumeroTarjeta.Substring(contador - 1, 1));
                }

                contador++;
            }

            return new MascaraDto() { CadenaConMascara = mascara.ToString() };
        }

    }
}
