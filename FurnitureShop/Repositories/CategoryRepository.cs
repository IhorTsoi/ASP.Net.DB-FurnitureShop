using FurnitureShop.Models;
using FurnitureShop.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureShop.Repositories
{
    public class CategoryRepository : Repository<Category>
    {
        public override void Initialize()
        {
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    cmdText: "Select * from Category;",
                    connection: conn
                );
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int ID = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        //
                        Items.Add(new Category(ID, name));
                    }
                }
            }
        }

        public void Create(Category category)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "INSERT INTO Category VALUES (@name);"
            );
            command.Parameters.Add(new SqlParameter("@name", category.Name));
            Execute(command);
        }

        public void Update(Category category)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "UPDATE Category " +
                            "SET Name=@name " +
                            "WHERE ID=@id;"
            );
            command.Parameters.AddRange(new SqlParameter[] {new SqlParameter("@id", category.ID),
                new SqlParameter("@name", category.Name)});
            Execute(command);
        }

        public void Delete(int ID)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "DELETE FROM Category WHERE ID = (@id);"
            );
            command.Parameters.Add(new SqlParameter("@id", ID));
            Execute(command);
        }


        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendLine(">> Category repository initialized:\n");
            foreach (var item in Items)
            {
                res.AppendLine(item.ToString());
            }
            return res.ToString();
        }

    }
}
