using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WebTestAssesment.Models;

namespace WebTestAssesment
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

	#region IIS-Session - Database Config
			builder.Services.AddSession(s => { s.IdleTimeout = System.TimeSpan.FromHours(6); });
			builder.Services.Configure<IISServerOptions>(op =>
			{
				op.AllowSynchronousIO = true;
				op.MaxRequestBodySize = 52428800;
			});

			string dbCon = builder.Configuration.GetValue<string>("DbConnections:Local");
			string host = builder.Configuration.GetValue<string>("Host:Name");


			builder.Services.AddDbContext<TestAssesmentDbContext>(op =>
					op.UseSqlServer(dbCon, x => x.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null).MigrationsAssembly("BeratenTribalCourtDataAccess")
					.CommandTimeout(90).MinBatchSize(1).MaxBatchSize(40).UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));


			#endregion
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Students}/{action=Index}/{id?}");

			app.Run();
		}
	}
}