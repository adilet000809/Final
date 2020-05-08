using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Final.Models;
using Final.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Final.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/")]
    public class MainController : ControllerBase
    {
        private readonly ITireRepository _tireRepository;
        private readonly IWheelRepository _wheelRepository;

        public MainController( 
            ITireRepository tireRepository,
            IWheelRepository wheelRepository
            )
        {
            _tireRepository = tireRepository;
            _wheelRepository = wheelRepository;
        }
        
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
    }
    
}