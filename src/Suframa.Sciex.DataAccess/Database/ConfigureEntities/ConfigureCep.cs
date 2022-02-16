namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureCep(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CepEntity>()
                .Property(e => e.Codigo)
                .HasPrecision(8, 0);

            modelBuilder.Entity<CepEntity>()
                .Property(e => e.Endereco)
                .IsUnicode(false);

            modelBuilder.Entity<CepEntity>()
                .Property(e => e.Logradouro)
                .IsUnicode(false);

            modelBuilder.Entity<CepEntity>()
                .Property(e => e.Bairro)
                .IsUnicode(false);
        }
    }
}