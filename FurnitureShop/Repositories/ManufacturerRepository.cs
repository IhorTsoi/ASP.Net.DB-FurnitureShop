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
    public class ManufacturerRepository : Repository<Manufacturer>
    {
        public override void Initialize()
        {
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    cmdText: "Select * from Manufacturer;",
                    connection: conn
                );
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int ID = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string description = reader.GetString(2);
                        string contacts = reader.IsDBNull(3) ? "-" : reader.GetString(3);
                        string image = reader.IsDBNull(4) ? "-" : reader.GetString(4);
                        //
                        Items.Add(new Manufacturer(ID, name, description, contacts, image));
                    }
                }
            }
        }

        public void Create(Manufacturer manufacturer)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "INSERT INTO Manufacturer (Name, Description, Contacts, Image)" +
                         " VALUES (@name, @desc, @contacts, @image);"
            );
            command.Parameters.Add(new SqlParameter("@name", manufacturer.Name));
            command.Parameters.Add(new SqlParameter("@desc", manufacturer.Description));
            command.Parameters.Add(new SqlParameter("@contacts", manufacturer.Contacts ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@image", manufacturer.Image ?? (object)DBNull.Value));
            Execute(command);
        }

        public void Update(Manufacturer manufacturer)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "UPDATE Manufacturer" +
                            "SET Name=@name," +
                            "Description=@desc," +
                            "Contacts=@contacts," +
                            "Image=@image" +
                            "WHERE ID=@id;"
            );
            command.Parameters.AddRange(new SqlParameter[] {
                new SqlParameter("@id", manufacturer.ID),
                new SqlParameter("@name", manufacturer.Name),
                new SqlParameter("@desc", manufacturer.Description),
                new SqlParameter("@contacts", manufacturer.Contacts),
                new SqlParameter("@image", manufacturer.Image) });
            Execute(command);
        }

        public void Delete(int ID)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "DELETE FROM Manufacturer WHERE ID = (@id);"
            );
            command.Parameters.Add(new SqlParameter("@id", ID));
            Execute(command);
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendLine(">> Manufacturer repository initialized:\n");
            foreach (var item in Items)
            {
                res.AppendLine(item.ToString());
            }
            return res.ToString();
        }
    }
}
