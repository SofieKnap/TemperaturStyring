using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace TemperaturStyring.Models
{
    public class ThermometerDbContext : DbContext
    {
        public ThermometerDbContext(DbContextOptions<ThermometerDbContext> options) : base(options)
        { 
        }
        
                    public DbSet<Thermometer> Thermometers { get; set; }

    }

}

