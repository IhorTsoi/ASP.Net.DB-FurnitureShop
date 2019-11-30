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
        private readonly int userId;
        private readonly SqlCommand cmd;

        public OrderRepository()
        {
            cmd = new SqlCommand("Select * from OrdersAll");
        }

        public OrderRepository(int userId)
        {
            this.userId = userId;
            cmd = new SqlCommand(cmdText: "Select * from OrdersAll Where BuyerId = @id;");
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
                            string buyerName = reader.GetString(1);
                            int orderHeaderId = reader.GetInt32(2);
                            DateTime? date = reader.IsDBNull(3) ? null : (DateTime?)reader.GetDateTime(3);
                            prev = new OrderHeader(orderHeaderId, date, buyerID);
                            prev.Buyer = new Models.Users.Buyer(buyerName, null, null);
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

        public void DeleteFromCart(string vendor)
        {
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    cmdText: "delete_from_cart",
                    connection: conn
                );
                command.CommandType = System.Data.CommandType.StoredProcedure;
                //
                command.Parameters.Add(new SqlParameter("@id", userId));
                command.Parameters.Add(new SqlParameter("@vendor_code", vendor));
                //
                SqlParameter ret = new SqlParameter();
                ret.Direction = System.Data.ParameterDirection.ReturnValue;
                command.Parameters.Add(ret);
                //
                command.ExecuteNonQuery();
                if ((int)ret.Value == 1)
                {
                    throw new Exception("Can't delete from cart: no such order detail, furniture or user.");
                } // 0 => ok
            }
        }

        public void AddToCart(string vendor)
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
                command.Parameters.Add(new SqlParameter("@id", userId));
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

        public bool ConfirmPurchase(out List<string> error_vendors)
        {
            // USAGE:
            // bool res = cr.ConfirmPurchase(Program.UserId, out List<string> errors);
            // if !res: see errors
            using (SqlConnection conn = new SqlConnection(Program.ConnectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(
                    cmdText: "confirm_purchase",
                    connection: conn);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                //
                command.Parameters.Add(new SqlParameter("@id", userId));
                //
                SqlParameter ret = new SqlParameter(
                    parameterName: "@return_code",
                    System.Data.SqlDbType.Int);
                ret.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(ret);
                //
                SqlDataReader reader = command.ExecuteReader();
                error_vendors = null;
                if (reader.HasRows)
                {
                    error_vendors = new List<string>();
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
                    case 0: // => ok
                        return true;
                    case 1: // => no such user or empty OH
                        throw new Exception("Can't confirm purchase. No such user or empty order.");
                    case 2: // => not enough in the stock
                        if (error_vendors == null)
                        {
                            throw new Exception("Confirm purchase. Resulted with return code = 2. But no further info were given.");
                        }
                        return false;
                    default:
                        throw new NotImplementedException();
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
