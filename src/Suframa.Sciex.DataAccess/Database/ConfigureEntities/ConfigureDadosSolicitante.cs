namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureDadosSolicitante(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DadosSolicitanteEntity>()
                .Property(e => e.Cpf)
                .IsUnicode(false);

            modelBuilder.Entity<DadosSolicitanteEntity>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<DadosSolicitanteEntity>()
                .Property(e => e.Email)
                .IsUnicode(false);
        }
    }
}