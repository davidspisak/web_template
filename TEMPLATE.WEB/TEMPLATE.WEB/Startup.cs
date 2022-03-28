using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TEMPLATE.Common;
using TEMPLATE.InfrastructureServices.Logging.DBLogger;
using TEMPLATE.InfrastructureServices.Logging.FileLogger;

namespace TEMPLATE.WEB
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			var appSettings = new ConfigurationBuilder()
				.SetBasePath(System.IO.Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();
			AppSettings.Instance = appSettings.GetSection("App").Get<AppSettings>();
			AppSettings.Instance.ApplicationVersion = typeof(Startup).Assembly.GetName().Version.ToString();

			Directory.CreateDirectory(AppSettings.Instance.AppLogger.LogPath);
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddHttpContextAccessor();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
		{
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();


			loggerFactory.AddProvider(new FileLoggerProvider(
				new FileLoggerConfig()
				{
					Configuration = configuration,
					EnableLogToFile = AppSettings.Instance.AppLogger.EnableLogToFile
				}, httpContextAccessor));

			loggerFactory.AddProvider(new DbLoggerProvider(
				new DbLoggerConfig()
				{
					ConnectionString = AppSettings.Instance.DbOptions.ConnectionString,
					EnableLogToDb = AppSettings.Instance.AppLogger.EnableLogToDb,
					PostgreSqlProvider = AppSettings.Instance.AppLogger.PostgreSqlProvider
				}, httpContextAccessor));


			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}
