using NLog;
using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Text;

namespace Suframa.Sciex.DataAccess.Database
{
    public class DatabaseInterceptor : IDbCommandInterceptor
    {
        private static ILogger logger = LogManager.GetLogger("db");

        public DatabaseInterceptor()
        {
            LogManager.ThrowExceptions = true;
        }

        private void WriteLog(DbCommand command, bool isAsync)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DbParameter item in command.Parameters)
                sb.AppendLine(item.ParameterName + ": " + item.Value);

            GlobalDiagnosticsContext.Set("CustomAsync", isAsync);
            var message = string.Format("{0}{1}{2}", command.CommandText, Environment.NewLine, sb.ToString());
            System.Diagnostics.Trace.WriteLine(message);
            logger.Info(message);

            //Log.Debug(m => m(command));

            // or use the verbose option
            /*if (Log.IsDebugEnabled)
            {
                Log.Debug(command);
            }*/
        }

        //private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            WriteLog(command, interceptionContext.IsAsync);
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            WriteLog(command, interceptionContext.IsAsync);
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            //WriteLog(string.Format("{0}IsAsync: {1}{2}Command Text: {3} {4}", Environment.NewLine, interceptionContext.IsAsync, Environment.NewLine, command.CommandText, LogParameter(command)));
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            WriteLog(command, interceptionContext.IsAsync);
        }
    }
}