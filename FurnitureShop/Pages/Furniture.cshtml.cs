using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FurnitureShop.Models;
using FurnitureShop.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FurnitureShop.Pages
{
    public class FurnitureModel : PageModel
    {
        public Furniture[] Furniture { get; set; }
        FurnitureRepository furnitureRepository { get; set; }
        public bool Sorted { get; set; }

        public void OnGet(string query, bool sort = false)
        {
            furnitureRepository = new FurnitureRepository(query: query, QueryMode.BySearchQuery);
            furnitureRepository.Initialize();
            Sorted = sort;
            if (Sorted)
            {
                Furniture = furnitureRepository.Items.OrderBy(f => f.Price).ToArray();
            }
            else
            {
                Furniture = furnitureRepository.Items.ToArray();
            }
        }

        public void OnGetCategory(int categoryId, bool sort = false)
        {
            furnitureRepository = new FurnitureRepository(query: categoryId.ToString(), QueryMode.Custom, "CategoryId = @query;");
            furnitureRepository.Initialize();
            Sorted = sort;
            if (Sorted)
            {
                Furniture = furnitureRepository.Items.OrderBy(f => f.Price).ToArray();
            }
            else
            {
                Furniture = furnitureRepository.Items.ToArray();
            }
        }

        public void OnGetCollection(int collectionId, bool sort = false)
        {
            furnitureRepository = new FurnitureRepository(query: collectionId.ToString(), QueryMode.Custom, "CollectionId = @query;");
            furnitureRepository.Initialize();
            Sorted = sort;
            if (Sorted)
            {
                Furniture = furnitureRepository.Items.OrderBy(f => f.Price).ToArray();
            }
            else
            {
                Furniture = furnitureRepository.Items.ToArray();
            }
        }

        public void OnGetManufacturer(int manufacturerId, bool sort = false)
        {
            furnitureRepository = new FurnitureRepository(query: manufacturerId.ToString(), QueryMode.Custom, "ManufacturerId = @query;");
            furnitureRepository.Initialize();
            Sorted = sort;
            if (Sorted)
            {
                Furniture = furnitureRepository.Items.OrderBy(f => f.Price).ToArray();
            }
            else
            {
                Furniture = furnitureRepository.Items.ToArray();
            }
        }
    }
}