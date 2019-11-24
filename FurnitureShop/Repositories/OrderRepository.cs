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
    public class OrderRepository : Repository<OrderHeader>
    {
        private readonly SqlCommand cmd;

        public OrderRepository(int userId)
        {
            cmd = new SqlCommand("Select * from OrdersAll Where BuyerId = @id;");
            cmd.Parameters.Add(new SqlParameter("@id", userId));
        }

        public override void Initialize()
        {
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                cmd.Connection = conn;
                SqlDataReader reader = cmd.ExecuteReader();

                OrderHeader prev = null;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (prev == null || !(prev.BuyerID == reader.GetInt32(0) && prev.ID == reader.GetInt32(2)))
                        {
                            int buyerID = reader.GetInt32(0);
                            int orderHeaderId = reader.GetInt32(2);
                            DateTime? date = reader.IsDBNull(3) ? null : (DateTime?)reader.GetDateTime(3);
                            prev = new OrderHeader(orderHeaderId, date, buyerID);
                            Items.Add(prev);
                        }

                        string vendor = reader.IsDBNull(4) ? null : reader.GetString(4);
                        string furnitureName = reader.IsDBNull(5) ? null : reader.GetString(5);
                        int? qty = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6);
                        decimal? price = reader.IsDBNull(7) ? null : (decimal?)reader.GetDecimal(7);
                        if (vendor == null)
                        {
                            continue;
                        }
                        OrderDetail od = new OrderDetail(prev.ID, vendor, new Furniture(vendor, furnitureName, (price == 0 ? 1 : (decimal)price), false, 1, null, null, null, 0, null), (int)qty);
                        prev.OrderDetails.AddDistinct(od);
                        //
                    }
                }
            }
        }

        public void DeleteFromCart(int id, string vendor)
        {
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    cmdText: "Select top 1 ID from get_pending_oh_od(@id);",
                    connection: conn
                );
                command.Parameters.Add(new SqlParameter("@id", id.ToString()));
                int oh_id = (int)command.ExecuteScalar();

                SqlCommand command1 = new SqlCommand(
                    cmdText: "Delete From OrderDetail Where OrderHeaderID=@oh_id AND VendorCode LIKE @vendor",
                    connection: conn);
                command1.Parameters.Add(new SqlParameter("@oh_id", oh_id));
                command1.Parameters.Add(new SqlParameter("@vendor", vendor));
                command1.ExecuteNonQuery();
            }
        }

        public void AddToCart(int id, string vendor)
        {
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    cmdText: "add_to_cart",
                    connection: conn
                );
                command.CommandType = System.Data.CommandType.StoredProcedure;
                //
                command.Parameters.Add(new SqlParameter("@id", id));
                //
                command.Parameters.Add(new SqlParameter("@vendor_code", vendor));
                //
                SqlParameter ret = new SqlParameter();
                ret.Direction = System.Data.ParameterDirection.ReturnValue;
                command.Parameters.Add(ret);
                //
                command.ExecuteNonQuery();
                if ((int)ret.Value == 1)
                {
                    throw new Exception("Can't add to cart: no such furniture or no such user.");
                } // 0 => ok
            }
        }

        public bool ConfirmPurchase(int id, List<string> error_vendors)
        {
            // USAGE:
            // List<string> errors = new List<string>();
            // bool res = cr.ConfirmPurchase(Program.UserId, errors);
            // if !res: see errors
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    cmdText: "confirm_purchase",
                    connection: conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                //
                command.Parameters.Add(new SqlParameter("@id", id));
                //
                SqlParameter ret = new SqlParameter(
                    parameterName: "@return_code",
                    System.Data.SqlDbType.Int);
                ret.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(ret);
                //
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string vendorCode = reader.GetString(0);
                        error_vendors.Add(vendorCode);
                    }
                }
                reader.Close();
                //
                switch ((int)ret.Value)
                {
                    case 1:
                        throw new Exception("Can't confirm purchase. No such user or empty order.");
                    case 2:
                        if (error_vendors.Count == 0)
                        {
                            throw new Exception("Confirm purchase. Resulted with return code = 2. But no further info were given.");
                        }
                        return false;
                    default:
                        return true;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendLine(">> Order repository initialized:\n");
            foreach (var item in Items)
            {
                res.AppendLine("===========================");
                res.AppendLine(item.ToString());
            }
            return res.ToString();
        }
    }
}
