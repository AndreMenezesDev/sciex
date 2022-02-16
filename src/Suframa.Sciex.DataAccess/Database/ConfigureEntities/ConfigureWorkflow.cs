using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.Entity;

namespace Suframa.Sciex.DataAccess.Database
{
    public partial class DatabaseContext : DbContext
    {
        private static void ConfigureWorkflow(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkflowProtocoloEntity>()
               .Property(e => e.Justificativa)
               .IsUnicode(false);
        }
    }
}