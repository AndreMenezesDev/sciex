namespace Suframa.Sciex.DataAccess
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
    /// </summary>
    public class UnitOfWorkSciex : IUnitOfWorkSciex
	{
        public ICommandStackSciex CommandStackSciex { get; }

        public IQueryStackSciex QueryStackSciex { get; }

        public UnitOfWorkSciex(IQueryStackSciex queryStackSciex, ICommandStackSciex commandStackSciex)
        {
            this.QueryStackSciex = queryStackSciex;
            this.CommandStackSciex = commandStackSciex;
        }
    }
}