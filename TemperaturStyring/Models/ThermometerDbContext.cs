using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TemperaturStyring.Models;

public class ThermometerDbContext : IdentityDbContext<ApplicationUser>
{
    public ThermometerDbContext(
        DbContextOptions<ThermometerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Thermometer> Thermometers => Set<Thermometer>();
}