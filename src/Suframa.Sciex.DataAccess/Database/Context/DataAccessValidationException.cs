using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace Suframa.Sciex.DataAccess.Database.Context
{
    [Serializable]
    public class DataAccessValidationException : Exception
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IEnumerable<DbEntityValidationResult> err;

        public override string Message
        {
            get
            {
                return EntityErrorMessageHelper.FormatMessage(err);
            }
        }

        public DataAccessValidationException(IEnumerable<DbEntityValidationResult> err)
        {
            this.err = err;
            logger.Error(this.Message);
        }
    }
}