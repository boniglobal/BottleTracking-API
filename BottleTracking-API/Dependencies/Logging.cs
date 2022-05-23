using NpgsqlTypes;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using Serilog.Sinks.PostgreSQL.ColumnWriters;

namespace BottleTracking_API.Dependencies
{
    public static class Logging
    {
        public static void AddSerilogConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionstring = configuration.GetConnectionString("PostgreSqlConnection");

            IDictionary<string, ColumnWriterBase> logs = new Dictionary<string, ColumnWriterBase>()
            {
                {"id", new IdAutoIncrementColumnWriter() },
                {"category", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                {"endpoint", new SinglePropertyColumnWriter("endpoint", PropertyWriteMethod.Raw, NpgsqlDbType.Text)},
                {"body", new SinglePropertyColumnWriter("body", PropertyWriteMethod.ToString, NpgsqlDbType.Text) },
                {"user_id", new SinglePropertyColumnWriter("user_id", PropertyWriteMethod.Raw, NpgsqlDbType.Integer) },
                {"ip_address", new SinglePropertyColumnWriter("ip_address", PropertyWriteMethod.ToString, NpgsqlDbType.Text) },
                {"response_code", new SinglePropertyColumnWriter("response_code", PropertyWriteMethod.Raw, NpgsqlDbType.Integer) },
                {"response_message", new SinglePropertyColumnWriter("response_message", PropertyWriteMethod.ToString, NpgsqlDbType.Text) },
                {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                {"request_timestamp", new SinglePropertyColumnWriter("request_timestamp", PropertyWriteMethod.Raw, NpgsqlDbType.TimestampTz) },
                {"log_timestamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
            };


            services.AddLogging(builder =>
            {
                var logger = new LoggerConfiguration()
                                .WriteTo
                                .PostgreSQL(connectionstring,
                                            "logs",
                                            logs,
                                            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                                            needAutoCreateTable: true)
                                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
                                .CreateLogger();

                builder.AddSerilog(logger);
            });
        }
    }
}
