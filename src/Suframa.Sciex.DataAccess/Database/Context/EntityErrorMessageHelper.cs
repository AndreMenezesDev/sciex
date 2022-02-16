using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace Suframa.Sciex.DataAccess.Database.Context
{
    public static class EntityErrorMessageHelper
    {
        private static object GetComplexPropertyValue(DbPropertyValues propertyValues, string[] propertyChain)
        {
            var propertyName = propertyChain.First();
            return propertyChain.Count() == 1
                ? propertyValues[propertyName]
                : GetComplexPropertyValue((DbPropertyValues)propertyValues[propertyName], propertyChain.Skip(1).ToArray());
        }

        public static string FormatMessage(IEnumerable<DbEntityValidationResult> err)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var eve in err)
            {
                sb.AppendLine(string.Format("- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().FullName, eve.Entry.State));
                foreach (var ve in eve.ValidationErrors)
                {
                    object value;
                    if (ve.PropertyName.Contains("."))
                    {
                        var propertyChain = ve.PropertyName.Split('.');
                        var complexProperty = eve.Entry.CurrentValues.GetValue<DbPropertyValues>(propertyChain.First());
                        value = GetComplexPropertyValue(complexProperty, propertyChain.Skip(1).ToArray());
                    }
                    else
                    {
                        value = eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName);
                    }
                    sb.AppendLine(string.Format("-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                        ve.PropertyName,
                        value,
                        ve.ErrorMessage));
                }
            }

            return sb.ToString();
        }
    }
}