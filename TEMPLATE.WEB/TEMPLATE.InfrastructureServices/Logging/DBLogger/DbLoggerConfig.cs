using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEMPLATE.InfrastructureServices.Logging.DBLogger
{
	public class DbLoggerConfig
	{
		public string ConnectionString { get; set; }
		public bool EnableLogToDb { get; set; }
		public bool PostgreSqlProvider { get; set; }
	}
}
