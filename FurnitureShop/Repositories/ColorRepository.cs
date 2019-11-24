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
    public class ColorRepository : Repository<Color>
    {
        public override void Initialize()
        {
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    cmdText: "Select * from Color;",
                    connection: conn
                );
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int ID = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string rgb = reader.GetString(2);
                        //
                        Items.Add(new Color(ID, name, rgb));
                    }
                }
            }
        }

        public void Create(Color color)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "INSERT INTO Color VALUES (@name, @rgb);"
            );
            command.Parameters.Add(new SqlParameter("@name", color.Name));
            command.Parameters.Add(new SqlParameter("@rgb", color.RGB));
            Execute(command);
        }

        public void Update(Color color)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "UPDATE Color " +
                            "SET Name=@name," +
                            " RGB=@rgb" +
                            "WHERE ID=@id;"
            );
            command.Parameters.AddRange(new SqlParameter[] {new SqlParameter("@id", color.ID),
                new SqlParameter("@name", color.Name),
                new SqlParameter("@rgb", color.RGB)});
            Execute(command);
        }

        public void Delete(int ID)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "DELETE FROM Color WHERE ID = (@id);"
            );
            command.Parameters.Add(new SqlParameter("@id", ID));
            Execute(command);
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendLine(">> Color repository initialized:\n");
            foreach (var item in Items)
            {
                res.AppendLine(item.ToString());
            }
            return res.ToString();
        }
    }
}
