using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMPLATE.InfrastructureServices.Logging.DBLogger
{
	public class DbLogger : Logger
	{
		private readonly DbLoggerConfig _config;

		public DbLogger(string name, DbLoggerConfig config, IHttpContextAccessor httpContextAccessor) : base(name, httpContextAccessor)
		{
			_config = config;
		}

		public override bool IsEnabled(LogLevel logLevel)
		{
			return (logLevel != LogLevel.None && _config.EnableLogToDb);
		}

		public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
				return;

			var logMessage = LogMessage.CreateLogMessage(logLevel, state.ToString(), exception, _httpContextAccessor);

			if (_config.PostgreSqlProvider)
			{
				if (logLevel >= LogLevel.Warning)
					logMessage.ErrorId = PostgresInsertError(logMessage);
				else
					logMessage.ErrorId = PostgresInsertTrace(logMessage);

			}

		}

		private long? PostgresInsertTrace(ILogMessage logMessage)
		{
			long? errorId = null;

			using (var conn = new Npgsql.NpgsqlConnection(_config.ConnectionString))
			{
				conn.Open();

				using (var cmd = new Npgsql.NpgsqlCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = "INSERT INTO aud.\"Trace\"(\"Created\",\"Level\", \"IdLogLevel\", \"IdUser\", \"Message\", \"CallerMethodFullName\", " +
						"\"ClientIp\", \"ClientAgent\", \"RequestPath\", \"RequestId\", \"RequestMethod\", \"MachineName\", \"ProcesId\", \"ThreadId\") " +
						"VALUES (@created, @level, @levelId, @idUser, @msg, @sc, @ip, @agent, @reqPath, @reqId, @reqMethod, @machine, @procesId, @threadId) " +
						"RETURNING \"IdTrace\"";
					cmd.Parameters.AddWithValue("created", logMessage.CreatedAt);
					cmd.Parameters.AddWithValue("level", (object)logMessage.LogLevel.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("levelId", (object)logMessage.LogLevelId ?? DBNull.Value);
					cmd.Parameters.AddWithValue("idUser", (object)logMessage.IdUser ?? DBNull.Value);
					cmd.Parameters.AddWithValue("msg", (object)logMessage.Message ?? string.Empty);
					cmd.Parameters.AddWithValue("sc", (object)logMessage.CallerMethodFullName?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("ip", (object)logMessage.ClientIp?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("agent", (object)logMessage.ClientAgent?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("reqPath", (object)logMessage.RequestPath?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("reqId", (object)logMessage.RequestId?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("reqMethod", (object)logMessage.RequestMethod?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("machine", (object)logMessage.MachineName?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("procesId", (object)logMessage.ProcesId?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("threadId", (object)logMessage.ThreadId?.ToString() ?? string.Empty);
					errorId = (long?)cmd.ExecuteScalar();
				}

				conn.Close();
			}

			return errorId;
		}

		private long? PostgresInsertError(ILogMessage logMessage)
		{
			long? errorId = null;

			using (var conn = new Npgsql.NpgsqlConnection(_config.ConnectionString))
			{
				conn.Open();

				using (var cmd = new Npgsql.NpgsqlCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = "INSERT INTO aud.\"Error\"(\"Created\", \"Level\", \"IdLogLevel\", \"IdUser\", \"Message\", \"ExceptionMessage\", \"Detail\", " +
						"\"CallerMethodFullName\", " +
						"\"ClientIp\", \"ClientAgent\", \"RequestPath\", \"RequestId\", \"RequestMethod\", \"MachineName\", \"ProcesId\", \"ThreadId\", \"Exception\") " +
						"VALUES (@created, @level, @levelId, @idUser, @msg, @exception, @detail, @sc, @ip, @agent, @reqPath, @reqId, @reqMethod, @machine, @procesId, @threadId, @ex) " +
						"RETURNING \"IdError\"";
					cmd.Parameters.AddWithValue("created", logMessage.CreatedAt);
					cmd.Parameters.AddWithValue("level", (object)logMessage.LogLevel.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("levelId", (object)logMessage.LogLevelId ?? DBNull.Value);
					cmd.Parameters.AddWithValue("idUser", (object)logMessage.IdUser ?? DBNull.Value);
					cmd.Parameters.AddWithValue("msg", (object)logMessage.Message ?? string.Empty);
					cmd.Parameters.AddWithValue("exception", (object)logMessage.ExceptionMessage ?? DBNull.Value);
					cmd.Parameters.AddWithValue("detail", (object)logMessage.Exception?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("sc", (object)logMessage.CallerMethodFullName?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("ip", (object)logMessage.ClientIp?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("agent", (object)logMessage.ClientAgent?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("reqPath", (object)logMessage.RequestPath?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("reqId", (object)logMessage.RequestId?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("reqMethod", (object)logMessage.RequestMethod?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("machine", (object)logMessage.MachineName?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("procesId", (object)logMessage.ProcesId?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("threadId", (object)logMessage.ThreadId?.ToString() ?? string.Empty);
					cmd.Parameters.AddWithValue("ex", (object)logMessage.Exception?.ToString() ?? string.Empty);
					errorId = (long?)cmd.ExecuteScalar();
				}

				conn.Close();
			}

			return errorId;
		}
	}
}
