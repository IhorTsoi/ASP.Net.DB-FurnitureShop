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
    public class MaterialRepository : Repository<Material>
    {
        public override void Initialize()
        {
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    cmdText: "Select * from Material;",
                    connection: conn
                );
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int ID = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string image = reader.GetString(2);
                        //
                        Items.Add(new Material(ID, name, image));
                    }
                }
            }
        }

        public void Create(Material material)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "INSERT INTO Material VALUES (@name, @image);"
            );
            command.Parameters.Add(new SqlParameter("@name", material.Name));
            command.Parameters.Add(new SqlParameter("@image", material.Image));
            Execute(command);
        }

        public void Update(Material material)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "UPDATE Material" +
                         "SET Name=@name," +
                            "Image=@image" +
                         "WHERE ID=@id;"
            );
            command.Parameters.AddRange(new SqlParameter[] {
                new SqlParameter("@id", material.ID),
                new SqlParameter("@name", material.Name),
                new SqlParameter("@image", material.Image)});
            Execute(command);
        }

        public void Delete(int ID)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "DELETE FROM Material WHERE ID = (@id);"
            );
            command.Parameters.Add(new SqlParameter("@id", ID));
            Execute(command);
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendLine(">> Material repository initialized:\n");
            foreach (var item in Items)
            {
                res.AppendLine(item.ToString());
            }
            return res.ToString();
        }
    }
}
