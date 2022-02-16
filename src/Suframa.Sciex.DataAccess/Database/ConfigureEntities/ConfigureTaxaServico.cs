namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureTaxaServico(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaxaServicoEntity>()
               .Property(e => e.DescricaoMsgRetorno)
               .IsUnicode(false);

            modelBuilder.Entity<TaxaServicoEntity>()
                .Property(e => e.Extrato)
                .IsUnicode(false);
        }
    }
}