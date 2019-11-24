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
    public class CollectionRepository : Repository<Collection>
    {
        public override void Initialize()
        {
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    cmdText: "Select * from Collection;",
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
                        //
                        Items.Add(new Collection(ID, name, description));
                    }
                }
            }
        }

        public void Create(Collection collection)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "INSERT INTO Collection VALUES (@name, @desc);"
            );
            command.Parameters.Add(new SqlParameter("@name", collection.Name));
            command.Parameters.Add(new SqlParameter("@desc", collection.Description));
            Execute(command);
        }

        public void Update(Collection collection)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "UPDATE Collection " +
                            "SET Name=@name," +
                            "Description=@desc " +
                            "WHERE ID=@id;"
            );
            command.Parameters.AddRange(new SqlParameter[] {new SqlParameter("@id", collection.ID),
                new SqlParameter("@name", collection.Name),
                new SqlParameter("@desc", collection.Description)});
            Execute(command);
        }

        public void Delete(int ID)
        {
            SqlCommand command = new SqlCommand(
                cmdText: "DELETE FROM Collection WHERE ID = (@id);"
            );
            command.Parameters.Add(new SqlParameter("@id", ID));
            Execute(command);
        }


        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendLine(">> Collection repository initialized:\n");
            foreach (var item in Items)
            {
                res.AppendLine(item.ToString());
            }
            return res.ToString();
        }
    }
}
