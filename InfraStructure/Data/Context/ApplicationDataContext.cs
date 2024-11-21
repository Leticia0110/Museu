using Microsoft.EntityFrameworkCore;
using MuseuAMSG3.Models;

namespace MuseuAMSG3.InfraStructure.Data.Context
{ 
            public class ApplicationDataContext : DbContext
{
        public ApplicationDataContext()
        {
        }

        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options)
    {

    }

    public DbSet<Cadastro> Cadastro { get; set; }
    public DbSet<Bilheteria> Bilheteria { get; set; }
    public DbSet<Login> Login { get; set; }
    }
}

