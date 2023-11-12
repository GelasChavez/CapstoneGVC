using CapstoneGVC.Contracts.DataTransferObjects;
using CapstoneGVC.Contracts.DataTransferObjects.ViewModels;
using CapstoneGVC.Contracts.DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneGVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            TarjetaDto tarjetaDto = new TarjetaDto();
            MascaraDto mascaraDto = new MascaraDto();
            EncriptadorDto tarjetaConHash = new EncriptadorDto();
            EncriptadorDto tarjetaEncriptada = new EncriptadorDto();
            EncriptadorDto tarjetaDesencriptada = new EncriptadorDto();
            EncriptadorDto hashTarjetaDesencriptada = new EncriptadorDto();

            TarjetaVm tarjetaViewModel = new TarjetaVm()
            {
                Mascara = mascaraDto,
                Tarjeta = tarjetaDto,
                TarjetaConHash = tarjetaConHash,
                TarjetaEncriptada = tarjetaEncriptada,
                TarjetaDesencriptada = tarjetaDesencriptada,
                HashDeTarjetaDesencriptada = hashTarjetaDesencriptada,
                HashsCoinciden = false
            };
            
            return View(tarjetaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GeneraProcesoCifrado(TarjetaVm tarjetaModel) 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Ingresa datos válidos");
                    throw new ArgumentException("Ingresa datos válidos");
                }

                TarjetaVm tarjetaViewModel = new TarjetaVm();

                tarjetaViewModel.Tarjeta = tarjetaModel.Tarjeta;

                //ENMASCARA EL NUMERO DE LA TAREJTA
                var mascara = _unitOfWork.Enmascarador.EnmascaraNumeroTarjeta(tarjetaViewModel.Tarjeta);
                tarjetaViewModel.Mascara = mascara;

                //OBTIENE EL HASH DEL NUMERO DE LA TARJETA
                EncriptadorDto tarjetaSinEncriptar = new EncriptadorDto() { CadenaSinEncriptar = tarjetaViewModel.Tarjeta.NumeroTarjeta };
                
                var tarjetaConHash = _unitOfWork.Encriptador.ObtenerHash(tarjetaSinEncriptar);
                tarjetaViewModel.TarjetaConHash = tarjetaConHash;

                //ENCRIPTA EL NUMERO DE LA TARJETA
                var tarjetaEncriptada = _unitOfWork.Encriptador.EncriptarCadena(tarjetaSinEncriptar);
                tarjetaViewModel.TarjetaEncriptada = tarjetaEncriptada;

                //DESENCRIPTA EL NUMERO DE TARJETA ENCRIPTADO
                var tarjetaDesencriptada = _unitOfWork.Encriptador.DesencriptarCadena(tarjetaEncriptada);
                tarjetaViewModel.TarjetaDesencriptada = tarjetaDesencriptada;

                //OBTIENE EL HASH DE LA CADENA DESENCRIPTADA
                var hashDeTarjetaDesencriptada = _unitOfWork.Encriptador.ObtenerHash(tarjetaDesencriptada);
                tarjetaViewModel.HashDeTarjetaDesencriptada = hashDeTarjetaDesencriptada;

                //VALIDAR HASH
                if (tarjetaConHash.Hash.Equals(hashDeTarjetaDesencriptada.Hash))
                {
                    tarjetaViewModel.HashsCoinciden = true;
                }

                return View("Index", tarjetaViewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                ErrorDto errorDto = new ErrorDto()
                {
                    MensajeError = e.Message
                };
                return View("Error", errorDto);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}