using ArchivPoznamek.Models;
using Microsoft.EntityFrameworkCore;

namespace ArchivPoznamek.Data
{
    public class ArchivPoznamekData : DbContext
    {
        public DbSet<Uzivatel> Uzivatele { get; set; }
        public ArchivPoznamekData(DbContextOptions<ArchivPoznamekData> options) : base(options) { }

    }
}
