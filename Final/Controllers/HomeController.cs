using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Final.Models;
using Final.Repositories;

namespace Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITireRepository _tireRepository;
        private readonly IWheelRepository _wheelRepository;

        public HomeController(
            ILogger<HomeController> logger,
            ITireRepository tireRepository,
            IWheelRepository wheelRepository
            )
        {
            _logger = logger;
            _tireRepository = tireRepository;
            _wheelRepository = wheelRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult TireDetails(int id)
        {
            ViewBag.Tire = _tireRepository.GetTire(id);
            return View();
        }
    }
}