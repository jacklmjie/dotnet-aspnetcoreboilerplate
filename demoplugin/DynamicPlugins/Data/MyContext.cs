using DynamicPlugins.Models;
using Microsoft.EntityFrameworkCore;

namespace DynamicPlugins.Data
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }

        public DbSet<Plugin> Plugins { get; set; }
        public DbSet<PluginMigration> PluginMigrations { get; set; }
    }
}
