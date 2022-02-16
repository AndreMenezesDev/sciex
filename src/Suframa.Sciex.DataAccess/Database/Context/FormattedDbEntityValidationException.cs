using NLog;
using System;
using System.Data.Entity.Validation;

namespace Suframa.Sciex.DataAccess.Database.Context
{
	/// <summary>
	/// https://stackoverflow.com/questions/7795300/validation-failed-for-one-or-more-entities-see-entityvalidationerrors-propert
	/// </summary>
	[Serializable]
	public class FormattedDbEntityValidationException : Exception
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public override string Message
		{
			get
			{
				var innerException = InnerException as DbEntityValidationException;
				if (innerException == null) return base.Message;

				return EntityErrorMessageHelper.FormatMessage(innerException.EntityValidationErrors);
			}
		}

		public FormattedDbEntityValidationException(DbEntityValidationException innerException) :
					base(null, innerException)
		{
			logger.Error(innerException.Message);
			logger.Error(this.Message);
		}
	}
}