using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace TheWebShop.Models
{
    internal class BestSellingCategory
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public static List<BestSellingCategory> GetBestSellingCategories()
        {
            var connectionString = "data source=tcp:dbrobindemo.database.windows.net,1433;Initial Catalog=dbDemo;Persist Security Info=False;User ID=robinadmin;Password=Sverige123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            var sql = @$"  select c.Name as 'Name'
		                ,count(od.OrderId) as 'Quantity'
                        from OrderDetails od
                        join Products p on od.ProductId = p.Id
                        join Categories c on c.Id = p.CategoryId
                        group by c.Name
                        order by count(od.OrderId) desc";
            var bestSellingCategories = new List<BestSellingCategory>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                bestSellingCategories = connection.Query<BestSellingCategory>(sql).ToList();
                connection.Close();
            }
            return bestSellingCategories;
        }
    }
}
