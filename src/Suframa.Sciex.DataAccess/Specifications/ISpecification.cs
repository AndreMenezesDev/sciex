using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Suframa.Sciex.DataAccess
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }

        void AddInclude(Expression<Func<T, object>> includeExpression);
    }
}