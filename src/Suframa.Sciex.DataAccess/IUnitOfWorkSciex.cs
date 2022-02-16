namespace Suframa.Sciex.DataAccess
{
    public interface IUnitOfWorkSciex
    {
        ICommandStackSciex CommandStackSciex { get; }
		IQueryStackSciex QueryStackSciex { get; }
    }
}