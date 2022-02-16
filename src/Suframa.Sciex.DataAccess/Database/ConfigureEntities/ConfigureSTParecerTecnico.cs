namespace Suframa.Sciex.DataAccess.Database
{
    using Suframa.Sciex.DataAccess.Database.Entities;
    using System.Data.Entity;

    public partial class DatabaseContextSciex : DbContext
    {
        private static void ConfigureSTParecerTecnico(DbModelBuilder modelBuilder)
        {
			modelBuilder.Entity<ST_ParecerTecnicoEntity>();
        }
    }
}