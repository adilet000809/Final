using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Final.Models;
using Final.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Final.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ITireRepository _tireRepository;
        private readonly IWheelRepository _wheelRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public MainController( 
            ITireRepository tireRepository,
            IWheelRepository wheelRepository,
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            UserManager<IdentityUser> userManager
            )
        {
            _tireRepository = tireRepository;
            _wheelRepository = wheelRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _userManager = userManager;
        }

        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        
        [HttpGet("tires")]
        public TirePaginationViewModel GetAllTires(
            string season,
            string width,
            string height,
            string diameter,
            string page="1"
            )
        {
            
            int pageSize = 2;
            var products = _tireRepository.GetAllTires();
            if (season != null)
            {
                products = products.Where(tire => tire.Season == (Seasons)Enum.Parse(typeof(Seasons), season));
            }
            if (width != null)
            {
                products = products.Where(tire => tire.Width == int.Parse(width));
            }
            if (height != null)
            {
                products = products.Where(tire => tire.Height == int.Parse(height));
            }
            if (diameter != null)
            {
                products = products.Where(tire => tire.Diameter == diameter);
            }
            products = products.Skip(( int.Parse(page) - 1) * pageSize).Take(pageSize);
            var paginationModel = new PaginationModel(int.Parse(page), pageSize, products.Count());
            var ivm = new TirePaginationViewModel { PaginationModel = paginationModel, Products = products };
            return ivm;
        }
        
        [HttpGet("wheels")]
        public WheelPaginationViewModel GetAllWheels(
            string holeDiameter,
            string width,
            string hole,
            string diameter,
            string page="1"
        )
        {
            
            int pageSize = 2;
            var wheels = _wheelRepository.GetAllWheels();
            if (holeDiameter != null)
            {
                wheels = wheels.Where(wheel => wheel.HoleDiameter == double.Parse(holeDiameter));
            }
            if (width != null)
            {
                wheels = wheels.Where(wheel => wheel.Width == int.Parse(width));
            }
            if (hole != null)
            {
                wheels = wheels.Where(wheel => wheel.Hole == int.Parse(hole));
            }
            if (diameter != null)
            {
                wheels = wheels.Where(wheel => wheel.Diameter == double.Parse(diameter));
            }
            wheels = wheels.Skip(( int.Parse(page) - 1) * pageSize).Take(pageSize);
            var paginationModel = new PaginationModel(int.Parse(page), pageSize, wheels.Count());
            var ivm = new WheelPaginationViewModel { PaginationModel = paginationModel, Products = wheels };
            return ivm;
        }

        [HttpPost("addtocart")]
        public CartItem AddToCart(JsonElement request)
        {
            var productId = request.GetProperty("productId").GetString();
            var categoryId = request.GetProperty("categoryId").GetString();
            Product product;
            if (categoryId == "1")
            {
                product = _tireRepository.GetTire(int.Parse(productId));
            }
            else
            {
                product = _wheelRepository.GetWheel(int.Parse(productId));
            }

            if (product == null) return null;
            
            var user = GetCurrentUserAsync().Result;
            var cart = _cartRepository.GetAllCarts().LastOrDefault(c => c.CustomerId == user.Id && c.IsOrdered==false) ?? new Cart();
            cart.Customer = user;
            cart.CustomerId = user.Id;
            var cartItem = _cartItemRepository.GetAllCartItems()
                               .FirstOrDefault(c => c.ProductId == product.Id && c.CartId == cart.Id) ?? new CartItem();
            cartItem.Product = product;
            cartItem.Cart = cart;
            cartItem.Quantity++;
            cartItem.CartId = cart.Id;
            cartItem.ProductId = product.Id;
            _cartItemRepository.AddOrUpdate(cartItem);
            _cartRepository.AddOrUpdate(cart);
                
            return cartItem;

        }

        [HttpGet("cart")]
        public IEnumerable<CartItem> GetCart()
        {
            var user = GetCurrentUserAsync().Result;
            var cart = _cartRepository.GetAllCarts().LastOrDefault(c => c.CustomerId == user.Id && c.IsOrdered==false) ?? new Cart();
            var cartItems = _cartItemRepository.GetAllCartItems().Where(c => c.CartId == cart.Id);
            foreach (var cartItem in cartItems)
            {
                var product = _tireRepository.GetTire(cartItem.ProductId) ?? (Product) _wheelRepository.GetWheel(cartItem.ProductId);
                cartItem.Product = product;
            }
            _cartRepository.AddOrUpdate(cart);

            return cartItems;
        }
        
        [HttpDelete("cartitem/{cartItemId}")]
        public IActionResult   DeleteCartItem(int cartItemId)
        {
            var result = _cartItemRepository.Delete(cartItemId);
            if (result == null) return BadRequest("Item not found");  
            return Ok();
        }
        
    }
    
}