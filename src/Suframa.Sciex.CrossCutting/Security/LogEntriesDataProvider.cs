using Audit.Core;

namespace Suframa.Sciex.CrossCutting.Security
{
    public class LogEntriesDataProvider : AuditDataProvider
    {
        private NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public override object InsertEvent(AuditEvent auditEvent)
        {
            log.Debug(auditEvent.ToJson());
            return string.Empty;
        }

        // Replaces an existing event given the ID and the event
        public override void ReplaceEvent(object eventId, AuditEvent auditEvent)
        {
            log.Debug(auditEvent.ToJson());
        }
    }
}