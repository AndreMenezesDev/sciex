namespace Suframa.Sciex.DataAccess
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        public ICommandStack CommandStack { get; }

        public IQueryStack QueryStack { get; }

        public UnitOfWork(IQueryStack queryStack, ICommandStack commandStack)
        {
            this.QueryStack = queryStack;
            this.CommandStack = commandStack;
        }
    }
}