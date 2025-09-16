using Microsoft.EntityFrameworkCore;
using PasswordManager;
using PManager.Data;
using PManager.Interfaces.Services;
using PManager.Models.Configs;

namespace PManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.Configure<SQLConfigs>(builder.Configuration.GetSection("SQL"));

            builder.Services.AddDbContext<PManagerDbContext>(options => options.UseSqlServer(builder.Configuration.GetSection("SQL").GetValue<string>("ConnectionString")));
            builder.Services.AddScoped<IDataService, DataService>();
            builder.Services.AddScoped<IEncryptionService, EncryptionService>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CORS", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseCors("CORS");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
