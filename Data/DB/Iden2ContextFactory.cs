using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Data.DB
{
    public class Iden2ContextFactory : IDesignTimeDbContextFactory<Iden2Context>
    {
        public Iden2Context CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("Connectionstring");

            var optionsBuilder = new DbContextOptionsBuilder<Iden2Context>();
            optionsBuilder.UseSqlServer(connectionString);

            return new Iden2Context(optionsBuilder.Options);
        }
    }
}
