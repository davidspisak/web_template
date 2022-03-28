using Microsoft.Extensions.Logging;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMPLATE.InfrastructureServices.Logging
{
	public class LogLevelConverter
	{
		public static LogLevel ConvertLogLevel(LogEventLevel logLevel)
		{
			switch (logLevel)
			{
				case LogEventLevel.Information:
					return LogLevel.Information;
				case LogEventLevel.Error:
					return LogLevel.Error;
				case LogEventLevel.Fatal:
					return LogLevel.Critical;
				case LogEventLevel.Verbose:
					return LogLevel.Trace;
				case LogEventLevel.Debug:
					return LogLevel.Debug;
				case LogEventLevel.Warning:
					return LogLevel.Warning;
				default:
					return LogLevel.Warning;
			}
		}


		public static LogEventLevel ConvertToSerilogLogLevel(string logLevel)
		{
			switch (logLevel)
			{
				case "Information":
					return LogEventLevel.Information;
				case "Error":
					return LogEventLevel.Error;
				case "Critical":
					return LogEventLevel.Fatal;
				case "Trace":
					return LogEventLevel.Verbose;
				case "Debug":
					return LogEventLevel.Debug;
				case "Warning":
					return LogEventLevel.Warning;
				default:
					return LogEventLevel.Warning;
			}
		}


		public static LogLevel ConvertToMSLogLevel(string logLevel)
		{
			switch (logLevel)
			{
				case "Information":
					return LogLevel.Information;
				case "Error":
					return LogLevel.Error;
				case "Critical":
					return LogLevel.Critical;
				case "Trace":
					return LogLevel.Trace;
				case "Debug":
					return LogLevel.Debug;
				case "Warning":
					return LogLevel.Warning;
				default:
					return LogLevel.Warning;
			}
		}


	}
}
