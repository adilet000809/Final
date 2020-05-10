using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Final.Models;
using Final.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITireRepository _tireRepository;
        private readonly IWheelRepository _wheelRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            ITireRepository tireRepository,
            IWheelRepository wheelRepository,
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            IOrderRepository orderRepository,
            UserManager<IdentityUser> userManager
            )
        {
            _logger = logger;
            _tireRepository = tireRepository;
            _wheelRepository = wheelRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
        }
        
        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

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
        
        public IActionResult WheelDetails(int id)
        {
            ViewBag.Wheel = _wheelRepository.GetWheel(id);
            return View();
        }
        
        public IActionResult Cart()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MakeOrder()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MakeOrder(CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = GetCurrentUserAsync().Result;
                var cart = _cartRepository.GetAllCarts().Last(c => c.CustomerId == user.Id && c.IsOrdered==false);
                var order = new Order()
                {
                    Customer = user,
                    Cart = cart,
                    CustomerId = user.Id,
                    CartId = cart.Id,
                    Address = model.Address,
                    Phone = model.Phone
                };
                _orderRepository.Add(order);
                cart.IsOrdered = true;
                _cartRepository.Update(cart);
            }

            return RedirectToAction("Index");
        }
    }
}