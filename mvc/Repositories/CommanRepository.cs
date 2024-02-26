using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace mvc.Repositories
{
    public class CommanRepository
    {
        public readonly NpgsqlConnection conn;
         public CommanRepository()
            {
                IConfiguration myConfig = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
                                            

                conn = new NpgsqlConnection(myConfig.GetConnectionString("MyConnection"));
        }
    }
}