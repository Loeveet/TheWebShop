using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace TheWebShop.Models
{
    internal class ProductsPerCustomer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Products { get; set; }

        public static List<ProductsPerCustomer> GetProductsPerCustomer()
        {
            var connectionString = "data source=tcp:dbrobindemo.database.windows.net,1433;Initial Catalog=dbDemo;Persist Security Info=False;User ID=robinadmin;Password=Sverige123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var sql = @$"select c.FirstName as 'FirstName'
                        ,c.LastName as 'LastName'
		                ,Count(od.ProductId) as 'Products'
                        from Customers c
                        join Orders o on c.Id = o.CustomerId
                        join OrderDetails od on od.OrderId = o.Id
                        group by c.FirstName, c.LastName
                        order by Count(od.ProductId) desc";
            var products = new List<ProductsPerCustomer>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                products = connection.Query<ProductsPerCustomer>(sql).ToList();
                connection.Close();
            }
            return products;
        }
    }
}
