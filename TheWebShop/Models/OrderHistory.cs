using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWebShop.Models
{
    internal class OrderHistory
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime OrderDate { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }


        public static List<OrderHistory> GetOrderHistories(int orderId, int customerId)
        {
            var connectionString = "data source=tcp:dbrobindemo.database.windows.net,1433;Initial Catalog=dbDemo;Persist Security Info=False;User ID=robinadmin;Password=Sverige123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var sql = @$"SELECT
                                c.FirstName AS 'FirstName',
                                c.LastName AS 'LastName',
                                FORMAT(o.OrderDate, 'yyyy-MM-dd HH:mm:ss') AS 'OrderDate',
                                p.Name AS 'ProductName',
                                Count(p.Name) AS 'Quantity',
                                sum(od.UnitPrice) / COUNT(p.Name) AS 'UnitPrice',
                                SUM(od.UnitPrice) AS 'TotalPrice'
                            FROM
                                Orders o
                            JOIN
                                Customers c ON c.Id = o.CustomerId
                            JOIN
                                OrderDetails od ON o.Id = od.OrderId
                            JOIN
                                Products p ON p.Id = od.ProductId
                            WHERE
                                o.CustomerId = {customerId} AND
                                o.Id = {orderId}  
                            GROUP BY
                                p.Name,
                                c.FirstName,
                                c.LastName,
                                o.OrderDate";
            var orderHistories = new List<OrderHistory>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                orderHistories = connection.Query<OrderHistory>(sql).ToList();
                connection.Close();
            }
            return orderHistories;
        }
    }
}
