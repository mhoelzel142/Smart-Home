using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SmartHome.Model
{
    public class DeviceContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = Database.db");
        }
    }
}
