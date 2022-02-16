namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigurePorteEmpresa(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PorteEmpresaEntity>()
               .Property(e => e.Descricao)
               .IsUnicode(false);

            //modelBuilder.Entity<PorteEmpresaEntity>()
            //    .HasMany(e => e.PessoaJuridica)
            //    .WithRequired(e => e.PorteEmpresa)
            //    .WillCascadeOnDelete(false);
        }
    }
}