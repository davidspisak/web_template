using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TEMPLATE.InfrastructureServices.Logging
{
	public class LogMessage : ILogMessage
	{
		public DateTime CreatedAt { get; set; }
		public LogLevel LogLevel { get; set; }
		public int? LogLevelId { get; set; }
		public string Message { get; set; }
		public string ExceptionMessage { get; set; }
		public long? ErrorId { get; set; }
		public string User { get; set; }
		public int? IdUser { get; set; }
		public string SqlStatement { get; set; }
		public string CallerMethodFullName { get; set; }
		[JsonIgnore]
		public Exception Exception { get; set; }
		public string ClientIp { get; set; }
		public string Environment { get; set; }
		public string ClientAgent { get; set; }
		public string RequestPath { get; set; }
		public string RequestId { get; set; }
		public string RequestMethod { get; set; }
		public string MachineName { get; set; }
		public string ProcesId { get; set; }
		public string ThreadId { get; set; }

		public static ILogMessage CreateLogMessage(LogLevel logLevel, string message, Exception exception, IHttpContextAccessor _httpContextAccessor)
		{
			return new LogMessage()
			{
				CreatedAt = DateTime.UtcNow,
				Exception = exception,
				LogLevel = logLevel,
				LogLevelId = (int)logLevel,
				ExceptionMessage = exception?.Message ?? string.Empty,
				SqlStatement = exception is PostgresException ? ((PostgresException)exception).InternalQuery : string.Empty,
				CallerMethodFullName = exception?.TargetSite?.DeclaringType?.FullName + "." + exception?.TargetSite?.Name,
				User = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? string.Empty,
				IdUser = (_httpContextAccessor?.HttpContext?.User?.Identity?.Name != null)
					? (int?)_httpContextAccessor?.HttpContext?.Items[_httpContextAccessor?.HttpContext?.User?.Identity?.Name]
					: null,
				Message = message ?? string.Empty,
				ClientAgent = _httpContextAccessor?.HttpContext?.Request?.Headers["User-Agent"] ?? string.Empty,
				ClientIp = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty,
				RequestPath = _httpContextAccessor?.HttpContext?.Request?.Path ?? string.Empty,
				RequestMethod = _httpContextAccessor?.HttpContext?.Request?.Method ?? string.Empty,
				RequestId = _httpContextAccessor?.HttpContext?.TraceIdentifier ?? string.Empty,
				MachineName = System.Environment.MachineName?.ToString(),
				ProcesId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString() ?? string.Empty,
				ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() ?? string.Empty
			};
		}
	}
}
