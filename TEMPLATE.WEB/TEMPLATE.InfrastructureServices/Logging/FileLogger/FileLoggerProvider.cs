using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMPLATE.InfrastructureServices.Logging.FileLogger
{
	[ProviderAlias("FileLogger")]
	public class FileLoggerProvider : ILoggerProvider
	{
		private readonly FileLoggerConfig _config;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public FileLoggerProvider(FileLoggerConfig config, IHttpContextAccessor httpContextAccessor)
		{
			_config = config;
			_httpContextAccessor = httpContextAccessor;
		}

		public ILogger CreateLogger(string categoryName)
		{
			if (_config == null)
				throw new NullReferenceException($"{nameof(FileLoggerConfig)}");

			if (_httpContextAccessor == null)
				throw new NullReferenceException($"{nameof(IHttpContextAccessor)}");

			return new FileLogger(categoryName, _config, _httpContextAccessor);
		}

		public void Dispose()
		{

		}
	}
}
