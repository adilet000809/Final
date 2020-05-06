using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Final.Models;
using Final.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;

namespace Final.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController: Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHostingEnvironment _appEnvironment;

        public AdminController(RoleManager<IdentityRole> roleManager, 
            UserManager<IdentityUser> userManager, 
            ICategoryRepository categoryRepository, 
            IProductRepository productRepository,
            IHostingEnvironment appEnvironment)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _appEnvironment = appEnvironment;
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
        public IActionResult ListProducts()
        {
            return View(_productRepository.GetAllProduct());
        }
        
        [HttpGet]
        public IActionResult CreateProduct()
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategory(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                {
                    var uploadFolder = Path.Combine(_appEnvironment.ContentRootPath + "\\wwwroot\\img");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                    var path = Path.Combine(uploadFolder, uniqueFileName);
                    model.Image.CopyTo(new FileStream(path, FileMode.Create));
                    
                }

                var category = _categoryRepository.GetCategory(model.CategoryId);

                var product = new Product
                {
                    Name = model.Name,
                    Category = category,
                    Price = model.Price,
                    Discount = model.Discount,
                    Amount = model.Amount,
                    Image = uniqueFileName,
                    CategoryId = model.CategoryId,
                    
                };
                
                _productRepository.Add(product);
                return RedirectToAction("ListProducts");
            }

            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = _productRepository.GetProduct(id);

            if (product == null)
            {
                ViewBag.ErrorMessage = $"Product cannot be found.";
                return View("NotFound");
            }
            
            var model = new EditProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Amount = product.Amount,
                Price = product.Price,
                Discount = product.Discount,
                CategoryId = product.CategoryId,
                OldImage = product.Image,
                Image = null
                
            };

            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategory(), "Id", "Name");
            ViewBag.OldImage = product.Image;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(EditProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = _productRepository.GetProduct(model.Id);
                product.Name = model.Name;
                product.Amount = model.Amount;
                product.Price = model.Price;
                product.Discount = model.Discount;
                product.CategoryId = model.CategoryId;
                product.Category = _categoryRepository.GetCategory(model.CategoryId);

                if (model.Image != null)
                {
                    string uniqueFileName = null;
                    {
                        
                        var uploadFolder = Path.Combine(_appEnvironment.ContentRootPath + "\\wwwroot\\img");
                        var oldFile = Path.Combine(uploadFolder, model.OldImage);
                        System.IO.File.Delete(oldFile);
                        
                        
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                        var path = Path.Combine(uploadFolder, uniqueFileName);
                        model.Image.CopyTo(new FileStream(path, FileMode.Create));
                    
                    }
                    product.Image = uniqueFileName;
                }
                
                _productRepository.Update(product);
                return RedirectToAction("ListProducts");
            }

            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = _productRepository.GetProduct(id);

            if (product == null)
            {
                ViewBag.ErrorMessage = $"Product cannot be found.";
                return View("NotFound");
            }
            
            var uploadFolder = Path.Combine(_appEnvironment.ContentRootPath + "\\wwwroot\\img");
            var image = Path.Combine(uploadFolder, product.Image);
            System.IO.File.Delete(image);

            var result = _productRepository.Delete(id);
            

            if (result != null)
            {
                return RedirectToAction("ListProducts");
            }
            
            return RedirectToAction("ListProducts");
            
        }
        
    }
}