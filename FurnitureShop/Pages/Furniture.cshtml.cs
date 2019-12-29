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
        private FurnitureRepository furnitureRepository { get; set; }

        public void OnGet(string query)
        {
            furnitureRepository = new FurnitureRepository(QueryMode.BySearchQuery, query: query);
            furnitureRepository.Initialize();
            Furniture = furnitureRepository.Items.ToArray();
            ViewData["Subtitle"] = "Мебель | По запросу: " + query;
        }

        public void OnGetAll()
        {
            furnitureRepository = new FurnitureRepository();
            furnitureRepository.Initialize();
            Furniture = furnitureRepository.Items.ToArray();
            ViewData["Subtitle"] = "Мебель";
        }

        public void OnGetCategory(int categoryId)
        {
            furnitureRepository = new FurnitureRepository(QueryMode.Custom, query: categoryId.ToString(), args: "CategoryId = @query;");
            furnitureRepository.Initialize();
            Furniture = furnitureRepository.Items.ToArray();
            ViewData["Subtitle"] = "Мебель | Категория " + ((Furniture.Length == 0) ? "" : Furniture.First().Category.Name);
        }

        public void OnGetCollection(int collectionId)
        {
            furnitureRepository = new FurnitureRepository(QueryMode.Custom, query: collectionId.ToString(), args: "CollectionId = @query;");
            furnitureRepository.Initialize();
            Furniture = furnitureRepository.Items.ToArray();
            ViewData["Subtitle"] = "Мебель | Коллекция " + ((Furniture.Length == 0) ? "" : Furniture.First().Collection.Name);
        }

        public void OnGetManufacturer(int manufacturerId)
        {
            furnitureRepository = new FurnitureRepository(QueryMode.Custom, query: manufacturerId.ToString(), args: "ManufacturerId = @query;");
            furnitureRepository.Initialize();
            Furniture = furnitureRepository.Items.ToArray();
            ViewData["Subtitle"] = "Мебель | Производитель " + ((Furniture.Length == 0) ? "" : Furniture.First().Manufacturer.Name);
        }
    }
}