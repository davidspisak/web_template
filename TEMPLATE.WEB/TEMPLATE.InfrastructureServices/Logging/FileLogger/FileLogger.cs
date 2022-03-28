using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMPLATE.InfrastructureServices.Logging.FileLogger
{
	public class FileLogger : Logger
	{
		private readonly FileLoggerConfig _config;
		private readonly Serilog.Core.Logger _serilogLogger;

		public FileLogger(string name, FileLoggerConfig config, IHttpContextAccessor httpContextAccessor) : base(name, httpContextAccessor)
		{
			_config = config;
			_serilogLogger = CreateSerilogLogger();
		}

		private Serilog.Core.Logger CreateSerilogLogger()
		{
			if (_config == null || _config.Configuration == null)
				throw new NullReferenceException($"{nameof(FileLoggerConfig)}");

			return new LoggerConfiguration()
				.ReadFrom.Configuration(_config.Configuration)
				.Enrich.FromLogContext()
				.CreateLogger();
		}

		public override bool IsEnabled(LogLevel logLevel)
		{
			return (logLevel != LogLevel.None && _config.EnableLogToFile);
		}

		public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
				return;

			var logMessage = LogMessage.CreateLogMessage(logLevel, state.ToString(), exception, _httpContextAccessor);
			_serilogLogger.Write(LogLevelConverter.ConvertToSerilogLogLevel(logLevel.ToString()), "{@ILogMessage:lj}{NewLine}", logMessage);
		}
	}
}
