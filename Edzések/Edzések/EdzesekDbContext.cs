using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Edzések;

public class EdzesekDbContext(DbContextOptions<EdzesekDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Edzesek> Edzesek { get; set; }
}
