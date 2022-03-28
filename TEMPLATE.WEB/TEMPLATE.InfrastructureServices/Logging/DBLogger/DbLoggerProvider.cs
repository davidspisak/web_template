using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMPLATE.InfrastructureServices.Logging.DBLogger
{
	[ProviderAlias("DbLogger")]
	public class DbLoggerProvider : ILoggerProvider
	{
		private readonly DbLoggerConfig _config;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public DbLoggerProvider(DbLoggerConfig config, IHttpContextAccessor httpContextAccessor)
		{
			_config = config;
			_httpContextAccessor = httpContextAccessor;
		}

		public ILogger CreateLogger(string categoryName)
		{
			if (_config == null)
				throw new NullReferenceException($"{nameof(DbLoggerConfig)}");

			if (_httpContextAccessor == null)
				throw new NullReferenceException($"{nameof(IHttpContextAccessor)}");

			return new DbLogger(categoryName, _config, _httpContextAccessor);
		}

		public void Dispose()
		{
		}
	}
}
