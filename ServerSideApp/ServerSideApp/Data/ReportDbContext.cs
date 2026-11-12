using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ServerSideApp.Data {
    public class SqlDataConnectionDescription : DataConnection { }
    public class JsonDataConnectionDescription : DataConnection { }
    public abstract class DataConnection {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
    }

    public class ReportItem {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? DisplayName { get; set; }
        public byte[] LayoutData { get; set; } = null!;
    }

    public class ReportDbContext : DbContext {
        public DbSet<JsonDataConnectionDescription> JsonDataConnections { get; set; } = null!;
        public DbSet<SqlDataConnectionDescription> SqlDataConnections { get; set; } = null!;
        public DbSet<ReportItem> Reports { get; set; } = null!;
        public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options) {
        }
        public void InitializeDatabase() {
            Database.EnsureCreated();

            var nwindJsonDataConnectionName = "NWindProductsJson";
            if(!JsonDataConnections.Any(x => x.Name == nwindJsonDataConnectionName)) {
                var newData = new JsonDataConnectionDescription {
                    Name = nwindJsonDataConnectionName,
                    DisplayName = "Northwind Products (JSON)",
                    ConnectionString = "Uri=Data/nwind.json"
                };
                JsonDataConnections.Add(newData);
            }


            var nwindSqlDataConnectionName = "NWindConnectionString";
            if(!SqlDataConnections.Any(x => x.Name == nwindSqlDataConnectionName)) {
                var newData = new SqlDataConnectionDescription {
                    Name = nwindSqlDataConnectionName,
                    DisplayName = "Northwind Data Connection",
                    ConnectionString = "XpoProvider=SQLite;Data Source=|DataDirectory|Data/nwind.db"
                };
                SqlDataConnections.Add(newData);
            }

            var reportsDataConnectionName = "ReportsDataSqlite";
            if(!SqlDataConnections.Any(x => x.Name == reportsDataConnectionName)) {
                var newData = new SqlDataConnectionDescription {
                    Name = reportsDataConnectionName,
                    DisplayName = "Reports Data (Demo)",
                    ConnectionString = "XpoProvider=SQLite;Data Source=|DataDirectory|Data/reportsData.db"
                };
                SqlDataConnections.Add(newData);
            }
            SaveChanges();
        }
    }
}