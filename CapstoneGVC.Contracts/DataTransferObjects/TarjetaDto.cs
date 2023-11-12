using CapstoneGVC.Contracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace CapstoneGVC.Contracts.DataTransferObjects
{
    public class TarjetaDto
    {
        [Required(ErrorMessage = "Ingresa un numero de tarejta válido")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Solo se permiten valores numéricos")]
        [MaxLength((int)ConfiguracionTarjeta.DIGITOS_PERMITIDOS, ErrorMessage = "Ingresa {1} caracteres")]
        [MinLength((int)ConfiguracionTarjeta.DIGITOS_PERMITIDOS, ErrorMessage = "Ingresa {1} caracteres")]
        public string NumeroTarjeta { get; set; } = "";
    }
}
