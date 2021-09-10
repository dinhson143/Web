using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Web.Data.EF
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // thư mục hiện tại làm thư mục gốc WebCore.Data
                .AddJsonFile("appsetting.json") // Add file này vào
                .Build();

            var connetionString = configuration.GetConnectionString("DefaultConnectionString");
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connetionString);
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}