using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMPLATE.InfrastructureServices.Logging.FileLogger
{
	public class FileLoggerConfig
	{
		public IConfigurationRoot Configuration { get; set; }
		public bool EnableLogToFile { get; set; }

	}
}
