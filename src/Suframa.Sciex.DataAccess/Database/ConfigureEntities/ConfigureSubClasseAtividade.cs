namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureSubclasseAtividade(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubclasseAtividadeEntity>()
               .Property(e => e.Descricao)
               .IsUnicode(false);

            modelBuilder.Entity<SubclasseAtividadeEntity>()
                .HasMany(e => e.PessoaJuridicaAtividade)
                .WithRequired(e => e.SubClasseAtividade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SubclasseAtividadeEntity>()
                .HasMany(e => e.SetorAtividade)
                .WithRequired(e => e.SubclasseAtividade)
                .WillCascadeOnDelete(false);
        }
    }
}