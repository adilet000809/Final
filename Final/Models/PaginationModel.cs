using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Final.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Final.Models
{
    public class PaginationModel
    {
        
        public int PageNumber { get; set; } 
        public int PageSize { get; set; } 
        public int TotalItems { get; set; } 
        public int TotalPages { get; set; }

        public PaginationModel(int pageNumber, int pageSize, int totalItems)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = (int) Math.Ceiling(totalItems / (double)pageSize) + 1;
        }
    }
}