using Microsoft.EntityFrameworkCore;
using NimedeAPI.Modules;
namespace NimedeAPI.Data
{
    public class NimedeDbContext : DbContext
    {
        public DbSet<Nimi> Nimed { get; set; }
        public DbSet<EmakeelneNimi> EmakeelsedNimed { get; set; }
        public DbSet<VoorkeelneNimi> VoorkeelsedNimed { get; set; }

        public NimedeDbContext(DbContextOptions<NimedeDbContext> options) : base(options) { }
    }
}
