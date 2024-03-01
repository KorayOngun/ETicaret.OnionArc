using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace ETicaret.API.Configurations.ColumnWriters
{
    public class UsernameColumnWriter : ColumnWriterBase
    {
        public UsernameColumnWriter() : base(NpgsqlDbType.Varchar)
        {
        }

        public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
        {
            var (userName, value) = logEvent.Properties.FirstOrDefault(p => p.Key == "user_name");
            return value?.ToString() ?? null;
        }
    }
}
