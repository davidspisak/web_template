using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMPLATE.InfrastructureServices.Logging
{
	public abstract class Logger : ILogger
	{
		protected string _name;
		protected IHttpContextAccessor _httpContextAccessor;

		public Logger(string name, IHttpContextAccessor httpContextAccessor)
		{
			_name = name;
			_httpContextAccessor = httpContextAccessor;
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return null;
		}

		public virtual bool IsEnabled(LogLevel logLevel)
		{
			throw new NotImplementedException();
		}

		public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			throw new NotImplementedException();
		}
	}
}
