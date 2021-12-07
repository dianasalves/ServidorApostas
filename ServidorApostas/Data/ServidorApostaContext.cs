using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ServidorApostas.Models;

#nullable disable

namespace ServidorApostas.Data
{
    public partial class ServidorApostaContext : DbContext
    {
        public ServidorApostaContext()
        {
        }

        public ServidorApostaContext(DbContextOptions<ServidorApostaContext> options)
            : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("name=ServidorApostaContext");
        //    }
        //}

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

        //    OnModelCreatingPartial(modelBuilder);
        //}

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public virtual DbSet<ModelUtilizador> MUtilizador { get; set; }
        public virtual DbSet<ModelApostas> MApostas { get; set; }

    }

    
}
