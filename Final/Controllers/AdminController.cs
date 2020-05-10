using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Final.Models;
using Final.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace Final.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController: Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITireRepository _tireRepository;
        private readonly IWheelRepository _wheelRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private static IHostingEnvironment _appEnvironment;

        public AdminController(RoleManager<IdentityRole> roleManager, 
            UserManager<IdentityUser> userManager, 
            ICategoryRepository categoryRepository, 
            ITireRepository tireRepository,
            IWheelRepository wheelRepository,
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            IHostingEnvironment appEnvironment)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
            _tireRepository = tireRepository;
            _appEnvironment = appEnvironment;
            _wheelRepository = wheelRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }

        private static string UploadImageTire(CreateTireViewModel model)
        {
            var uploadFolder = Path.Combine(_appEnvironment.ContentRootPath + "\\wwwroot\\img\\tire");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
            var path = Path.Combine(uploadFolder, uniqueFileName);
            var fileStream = new FileStream(path, FileMode.Create);
            model.Image.CopyTo(fileStream);
            fileStream.Close();
            return uniqueFileName;
        }
        
        private static string UploadImageWheel(CreateWheelViewModel model)
        {
            var uploadFolder = Path.Combine(_appEnvironment.ContentRootPath + "\\wwwroot\\img\\wheel");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
            var path = Path.Combine(uploadFolder, uniqueFileName);
            var fileStream = new FileStream(path, FileMode.Create);
            model.Image.CopyTo(fileStream);
            fileStream.Close();
            return uniqueFileName;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Name = model.RoleName
                };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Admin");
                    
                }
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            return View(_roleManager.Roles);
        }

        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role cannot be found.";
                return View("NotFound");
            }
            
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName); 
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role cannot be found.";
                return View("NotFound");
            }

            role.Name = model.RoleName;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role= await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role cannot be found.";
                return View("NotFound");
            }

            foreach (var user in _userManager.Users)
            {

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {

                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            
            return RedirectToAction("ListRoles");
            
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role cannot be found";
                return View("NotFound");
            }
            
            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                
                model.Add(userRoleViewModel);
            }

            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role cannot be found";
                return View("NotFound");
            }

            for (var i = 0; i < model.Count; i++)
            {
                var user  = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                    
                } 
                else if(!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }

                if (result == null || !result.Succeeded) continue;
                if (i < (model.Count - 1))
                {
                    continue;
                }
                return RedirectToAction("EditRole", new {Id = roleId});

            }
            
            return RedirectToAction("EditRole", new {Id = roleId});
        }
        
        //Category
        
        [HttpGet]
        public IActionResult ListCategories()
        {
            return View(_categoryRepository.GetAllCategory());
        }
        
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = model.CategoryName
                };
                var result = _categoryRepository.Add(category);

                if (result != null)
                {
                    return RedirectToAction("ListCategories", "Admin");
                    
                }
                
            }

            return View(model);
        }
        
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = _categoryRepository.GetCategory(id);

            if (category == null)
            {
                ViewBag.ErrorMessage = $"Category cannot be found.";
                return View("NotFound");
            }
            
            var model = new EditCategoryViewModel
            {
                Id = category.Id,
                CategoryName = category.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(EditCategoryViewModel model)
        {
            var category = _categoryRepository.GetCategory(model.Id);

            if (category == null)
            {
                ViewBag.ErrorMessage = $"Category cannot be found.";
                return View("NotFound");
            }

            category.Name = model.CategoryName;
            var result = _categoryRepository.Update(category);

            if (result != null)
            {
                return RedirectToAction("ListCategories");
            }

            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = _categoryRepository.GetCategory(id);

            if (category == null)
            {
                ViewBag.ErrorMessage = $"Category cannot be found.";
                return View("NotFound");
            }

            var result = _categoryRepository.Delete(id);

            if (result != null)
            {
                return RedirectToAction("ListCategories");
            }
            
            return RedirectToAction("ListCategories");
            
        }
        
        //Product

        [HttpGet]
        public IActionResult ListTires()
        {
            return View(_tireRepository.GetAllTires());
        }
        
        [HttpGet]
        public IActionResult CreateTire()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTire(CreateTireViewModel model)
        {
            if (ModelState.IsValid)
            {

                var category = _categoryRepository.GetCategory(model.CategoryId);

                var tire = new Tire
                {
                    Name = model.Name,
                    Category = category,
                    Price = model.Price,
                    Width = model.Width,
                    Height = model.Height,
                    Diameter = model.Diameter,
                    Description = model.Description,
                    Image = UploadImageTire(model),
                    Season = model.Season,
                    CategoryId = model.CategoryId,
                };
                
                _tireRepository.Add(tire);
                return RedirectToAction("ListTires");
            }

            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> EditTire(int id)
        {
            var tire = _tireRepository.GetTire(id);

            if (tire == null)
            {
                ViewBag.ErrorMessage = $"Product cannot be found.";
                return View("NotFound");
            }
            
            var model = new EditTireViewModel
            {
                Id = tire.Id,
                Name = tire.Name,
                Price = tire.Price,
                Width = tire.Width,
                Height = tire.Height,
                Diameter = tire.Diameter,
                Description = tire.Description,
                CategoryId = tire.CategoryId,
                Season = tire.Season,
                OldImage = tire.Image,
                Image = null
                
            };

            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategory(), "Id", "Name");
            ViewBag.OldImage = tire.Image;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditTire(EditTireViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tire = _tireRepository.GetTire(model.Id);
                tire.Name = model.Name;
                tire.Width = model.Width;
                tire.Height = model.Height;
                tire.Description = model.Description;
                tire.Diameter = model.Diameter;
                tire.Season = model.Season;
                tire.Price = model.Price;
                tire.CategoryId = model.CategoryId;
                tire.Category = _categoryRepository.GetCategory(model.CategoryId);

                if (model.Image != null)
                {
                    tire.Image = UploadImageTire(model);
                    
                    var uploadFolder = Path.Combine(_appEnvironment.ContentRootPath + "\\wwwroot\\img\\tire");
                    var oldFile = Path.Combine(uploadFolder, model.OldImage);
                    System.IO.File.Delete(oldFile);
                    
                    
                }
                
                _tireRepository.Update(tire);
                return RedirectToAction("ListTires");
            }

            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteTire(int id)
        {
            var tire = _tireRepository.GetTire(id);

            if (tire == null)
            {
                ViewBag.ErrorMessage = $"Product cannot be found.";
                return View("NotFound");
            }
            
            var uploadFolder = Path.Combine(_appEnvironment.ContentRootPath + "\\wwwroot\\img\\tire");
            var image = Path.Combine(uploadFolder, tire.Image);
            System.IO.File.Delete(image);

            var result = _tireRepository.Delete(id);
            

            if (result != null)
            {
                return RedirectToAction("ListTires");
            }
            
            return RedirectToAction("ListTires");
            
        }
        
        [HttpGet]
        public IActionResult ListWheels()
        {
            return View(_wheelRepository.GetAllWheels());
        }
        
        [HttpGet]
        public IActionResult CreateWheel()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateWheel(CreateWheelViewModel model)
        {
            if (ModelState.IsValid)
            {

                var category = _categoryRepository.GetCategory(model.CategoryId);

                var wheel = new Wheel()
                {
                    Name = model.Name,
                    Category = category,
                    Price = model.Price,
                    Width = model.Width,
                    Hole = model.Hole,
                    HoleDiameter = model.HoleDiameter,
                    Description = model.Description,
                    Diameter = model.Diameter,
                    Image = UploadImageWheel(model),
                    CategoryId = model.CategoryId,
                };
                
                _wheelRepository.Add(wheel);
                return RedirectToAction("ListWheels");
            }

            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> EditWheel(int id)
        {
            var wheel = _wheelRepository.GetWheel(id);

            if (wheel == null)
            {
                ViewBag.ErrorMessage = $"Product cannot be found.";
                return View("NotFound");
            }
            
            var model = new EditWheelViewModel
            {
                Id = wheel.Id,
                Name = wheel.Name,
                Price = wheel.Price,
                Width = wheel.Width,
                Diameter = wheel.Diameter,
                Hole = wheel.Hole,
                HoleDiameter = wheel.HoleDiameter,
                Description = wheel.Description,
                CategoryId = wheel.CategoryId,
                OldImage = wheel.Image,
                Image = null
                
            };

            ViewBag.OldImage = wheel.Image;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditWheel(EditWheelViewModel model)
        {
            if (ModelState.IsValid)
            {
                var wheel = _wheelRepository.GetWheel(model.Id);
                wheel.Name = model.Name;
                wheel.Width = model.Width;
                wheel.HoleDiameter = model.HoleDiameter;
                wheel.Description = model.Description;
                wheel.Diameter = model.Diameter;
                wheel.Hole = model.Hole;
                wheel.Price = model.Price;
                wheel.CategoryId = model.CategoryId;
                wheel.Category = _categoryRepository.GetCategory(model.CategoryId);

                if (model.Image != null)
                {
                    var uploadFolder = Path.Combine(_appEnvironment.ContentRootPath + "\\wwwroot\\img\\wheel");
                    var oldFile = Path.Combine(uploadFolder, model.OldImage);
                    System.IO.File.Delete(oldFile);
                    
                    wheel.Image = UploadImageWheel(model);
                }
                
                _wheelRepository.Update(wheel);
                return RedirectToAction("ListWheels");
            }

            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteWheel(int id)
        {
            var wheel = _wheelRepository.GetWheel(id);

            if (wheel == null)
            {
                ViewBag.ErrorMessage = $"Product cannot be found.";
                return View("NotFound");
            }
            
            var uploadFolder = Path.Combine(_appEnvironment.ContentRootPath + "\\wwwroot\\img\\wheel");
            var image = Path.Combine(uploadFolder, wheel.Image);
            System.IO.File.Delete(image);

            var result = _wheelRepository.Delete(id);
            

            if (result != null)
            {
                return RedirectToAction("ListWheels");
            }
            
            return RedirectToAction("ListWheels");
            
        }

        [HttpGet]
        public IActionResult ListOrders()
        {
            var orders = _orderRepository.GetAllOrder();
            foreach (var order in orders)
            {
                order.Customer = _userManager.FindByIdAsync(order.CustomerId).Result;
            }
            
            return View(orders);
        }
        
        [HttpGet]
        public IActionResult OrderDetails(int id)
        {
            var order = _orderRepository.GetOrder(id);
            var cartItems = _cartItemRepository.GetAllCartItems().Where(c => c.CartId == order.CartId);
            var total = 0.0;
            foreach (var cartItem in cartItems)
            {
                var product = _tireRepository.GetTire(cartItem.ProductId) ?? (Product) _wheelRepository.GetWheel(cartItem.ProductId);
                cartItem.Product = product;
                total += product.Price;
            }

            order.Customer = _userManager.FindByIdAsync(order.CustomerId).Result;
            ViewBag.Order = order;
            ViewBag.Products = cartItems;
            ViewBag.Total = total;
            return order == null ? View("NotFound") : View();
        }
        
        [HttpGet]
        public IActionResult AcceptOrder(int id)
        {
            var order = _orderRepository.GetOrder(id);
            if (order != null)
            {
                order.IsAccepted = true;
                _orderRepository.Update(order);
                return RedirectToAction("ListOrders");
            }
            ViewBag.ErrorMessage = $"Order cannot be found";
            return View("NotFound");

        }
        
        [HttpGet]
        public IActionResult DeleteOrder(int id)
        {
            var order = _orderRepository.Delete(id);
            if (order != null) return RedirectToAction("ListOrders");
            ViewBag.ErrorMessage = $"Order cannot be found";
            return View("NotFound");

        }
        
    }
}