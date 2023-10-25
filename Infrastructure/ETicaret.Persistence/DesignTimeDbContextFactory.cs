using ETicaret.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ETicaretDbContext>
    {
        public ETicaretDbContext CreateDbContext(string[] args)
        {
            //ConfigurationManager configurationManager = new ConfigurationManager();
            //configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(),"../../Presentation/ETicaret.API"));
            //configurationManager.AddJsonFile("appsettings.json");

            DbContextOptionsBuilder<ETicaretDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql(Configurations.ConnectionString);
            return new ETicaretDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
