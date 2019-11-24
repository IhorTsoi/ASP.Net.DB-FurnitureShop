using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FurnitureShop.Models;
using FurnitureShop.Repositories.Base;

namespace FurnitureShop.Repositories
{
    public enum QueryMode
    {
        ByVendorCode,
        BySearchQuery,
        Custom
    }

    public class FurnitureRepository : Repository<Furniture>
    {
        private readonly SqlCommand cmd;

        public FurnitureRepository()
        {
            // default
            cmd = new SqlCommand("Select * from FurnitureAll;");
        }
        public FurnitureRepository(string query, QueryMode mode, params string[] args)
        {
            switch (mode)
            {
                case QueryMode.ByVendorCode:
                    cmd = new SqlCommand("Select * from FurnitureAll WHERE VendorCode LIKE @vendor;");
                    cmd.Parameters.Add(new SqlParameter("@vendor", query));
                    break;
                case QueryMode.BySearchQuery:
                    cmd = new SqlCommand(cmdText: "SELECT * FROM dbo.smart_search(@query) ORDER BY Degree DESC");
                    cmd.Parameters.Add(new SqlParameter("@query", query));
                    break;
                case QueryMode.Custom:
                    cmd = new SqlCommand("Select * from FurnitureAll WHERE " + args[0]); // "Category = @query"
                    cmd.Parameters.Add(new SqlParameter("@query", query));
                    break;
                default:
                    break;
            }
        }

        //

        public override void Initialize()
        {
            //
            CategoryRepository categoryRepository = new CategoryRepository();
            categoryRepository.Initialize();
            ManufacturerRepository manufacturerRepository = new ManufacturerRepository();
            manufacturerRepository.Initialize();
            CollectionRepository collectionRepository = new CollectionRepository();
            collectionRepository.Initialize();
            ColorRepository colorRepository = new ColorRepository();
            colorRepository.Initialize();
            MaterialRepository materialRepository = new MaterialRepository();
            materialRepository.Initialize();

            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                cmd.Connection = conn;
                SqlDataReader reader = cmd.ExecuteReader();
                //
                Furniture prev = null;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (prev == null || prev.VendorCode != reader.GetString(0))
                        {
                            // start parsing new instance
                            string vendorCode = reader.GetString(0);
                            string name = reader.GetString(1);
                            //
                            decimal price = reader.GetDecimal(2);
                            bool isOnSale = !reader.IsDBNull(3);
                            price = isOnSale ? reader.GetDecimal(3) : price;
                            //
                            int quantity = reader.GetInt32(4);
                            //
                            int categoryId = reader.GetInt32(5);
                            Category category = categoryRepository.Find(cat => cat.ID == categoryId);
                            //
                            int manufacturerId = reader.GetInt32(6);
                            Manufacturer manufacturer = manufacturerRepository.Find(man => man.ID == manufacturerId);
                            //
                            int collectionId = reader.GetInt32(7);
                            Collection collection = collectionRepository.Find(col => col.ID == collectionId);
                            //
                            int rate = reader.GetInt32(8);
                            string image = reader.GetString(9);
                            // update prev & add to list
                            prev = new Furniture(vendorCode, name, price, isOnSale, quantity, category,
                                manufacturer, collection, rate, image);
                            Items.Add(prev);
                        }
                        //
                        if (!reader.IsDBNull(10))
                        {
                            int colorId = reader.GetInt32(10);
                            prev.Colors.AddDistinct(colorRepository.Find(c => c.ID == colorId));
                        }
                        //
                        if (!reader.IsDBNull(11))
                        {
                            int materialId = reader.GetInt32(11);
                            prev.Materials.AddDistinct(materialRepository.Find(m => m.ID == materialId));
                        }
                        //
                        if (!reader.IsDBNull(12))
                        {
                            string sizeType = reader.GetString(12);
                            int width = reader.GetInt32(13);
                            int depth = reader.GetInt32(14);
                            int height = reader.GetInt32(15);
                            Size size = new Size(prev.VendorCode, sizeType, width, height, depth);
                            prev.Sizes.AddDistinct(size);
                        }
                    }
                }
            }
        }

        public void Create(Furniture furniture)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "INSERT INTO Furniture " +
                            "VALUES (@vendor, @name, @price, NULL, @qty, @catId, @manId, @colId, 0, @image);"
            );
            command.Parameters.Add(new SqlParameter("@vendor", furniture.VendorCode));
            command.Parameters.Add(new SqlParameter("@name", furniture.Name));
            command.Parameters.Add(new SqlParameter("@price", furniture.Price));
            command.Parameters.Add(new SqlParameter("@qty", furniture.Quantity));
            command.Parameters.Add(new SqlParameter("@catId", furniture.CategoryID));
            command.Parameters.Add(new SqlParameter("@manId", furniture.ManufacturerID));
            command.Parameters.Add(new SqlParameter("@colId", furniture.CollectionID));
            command.Parameters.Add(new SqlParameter("@image", furniture.Image));
            Execute(command);
        }

        public void Update(Furniture furniture)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "UPDATE Furniture " +
                            "SET Name=@name," +
                            "Price=@price," +
                            "Quantity=@qty," +
                            "CategoryID=@catId," +
                            "ManufacturerID=@manId," +
                            "CollectionID=@colId," +
                            "Image=@image " +
                            "WHERE VendorCode=@vendor;"
            );
            command.Parameters.Add(new SqlParameter("@vendor", furniture.VendorCode));
            command.Parameters.Add(new SqlParameter("@name", furniture.Name));
            command.Parameters.Add(new SqlParameter("@price", furniture.Price));
            command.Parameters.Add(new SqlParameter("@qty", furniture.Quantity));
            command.Parameters.Add(new SqlParameter("@catId", furniture.Category?.ID ?? furniture.CategoryID));
            command.Parameters.Add(new SqlParameter("@manId", furniture.Manufacturer?.ID ?? furniture.ManufacturerID));
            command.Parameters.Add(new SqlParameter("@colId", furniture.Collection?.ID ?? furniture.CollectionID));
            command.Parameters.Add(new SqlParameter("@image", furniture.Image));
            Execute(command);
        }

        public void Delete(string vendorCode)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "DELETE FROM Furniture " +
                            "WHERE VendorCode LIKE @vendor;"
            );
            command.Parameters.Add(new SqlParameter("@vendor", vendorCode));
            Execute(command);
        }

        public void AddColor(string VendorCode, int ID)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "INSERT INTO FurnitureColor " +
                            "VALUES (@vendor, @id)"
            );
            command.Parameters.Add(new SqlParameter("@vendor", VendorCode));
            command.Parameters.Add(new SqlParameter("@id", ID));
            Execute(command);
        }

        public void DeleteColor(string VendorCode, int ID)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "DELETE FROM FurnitureColor " +
                            "WHERE VendorCode LIKE @vendor AND ColorID = @id"
            );
            command.Parameters.Add(new SqlParameter("@vendor", VendorCode));
            command.Parameters.Add(new SqlParameter("@id", ID));
            Execute(command);
        }

        public void AddMaterial(string VendorCode, int ID)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "INSERT INTO FurnitureMaterial " +
                            "VALUES (@vendor, @id)"
            );
            command.Parameters.Add(new SqlParameter("@vendor", VendorCode));
            command.Parameters.Add(new SqlParameter("@id", ID));
            Execute(command);
        }

        public void DeleteMaterial(string VendorCode, int ID)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "DELETE FROM FurnitureMaterial " +
                            "WHERE VendorCode LIKE @vendor AND MaterialID = @id"
            );
            command.Parameters.Add(new SqlParameter("@vendor", VendorCode));
            command.Parameters.Add(new SqlParameter("@id", ID));
            Execute(command);
        }

        public void AddSize(string VendorCode, Size size)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "INSERT INTO Size " +
                            "VALUES (@vendor, @type, @w, @h, @d)"
            );
            command.Parameters.Add(new SqlParameter("@vendor", VendorCode));
            command.Parameters.Add(new SqlParameter("@type", size.Type));
            command.Parameters.Add(new SqlParameter("@w", size.Width));
            command.Parameters.Add(new SqlParameter("@h", size.Height));
            command.Parameters.Add(new SqlParameter("@d", size.Depth));
            Execute(command);
        }

        public void DeleteSize(string vendorCode, string Type)
        {
            SqlCommand command = new SqlCommand(
               cmdText: "DELETE FROM Size " +
                           "WHERE VendorCode LIKE @vendor AND Type LIKE @type"
           );
            command.Parameters.Add(new SqlParameter("@vendor", vendorCode));
            command.Parameters.Add(new SqlParameter("@type", Type));
            Execute(command);
        }

        public void SetDiscount(string vendorCode, decimal newPrice)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "Update Furniture Set PriceDiscount=@newPrice Where VendorCode LIKE @vendor"
            );
            command.Parameters.Add(new SqlParameter("@newPrice", newPrice));
            command.Parameters.Add(new SqlParameter("@vendor", vendorCode));
            Execute(command);
        }

        public void RemoveDiscount(string vendorCode)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "Update Furniture Set PriceDiscount=NULL Where VendorCode LIKE @vendor"
            );
            command.Parameters.Add(new SqlParameter("@vendor", vendorCode));
            Execute(command);
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendLine(">> Furniture repository initialized:\n");
            foreach (var item in Items)
            {
                res.AppendLine(item.ToString());
            }
            return res.ToString();
        }
    }
}
