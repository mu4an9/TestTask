using System.Collections.Generic;
using System.Data.Entity;
using Url_Shortener.Models;

namespace Url_Shortener.Data
{

    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext() : base("YourConnectionString")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AppDbContext>());
        }
    }

}

