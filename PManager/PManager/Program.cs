using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PasswordManager;
using PManager.Data;
using PManager.Interfaces.Services;
using PManager.Models.Configs;
using PManager.Services;
using System.Text;

namespace PManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.Configure<SQLConfigs>(builder.Configuration.GetSection("SQL"));
            builder.Services.Configure<JWTConfigs>(builder.Configuration.GetSection("JWT"));

            builder.Services.AddDbContext<PManagerDbContext>(options => options.UseSqlServer(builder.Configuration.GetSection("SQL").GetValue<string>("ConnectionString")));
            builder.Services.AddScoped<IDataService, DataService>();
            builder.Services.AddScoped<IEncryptionService, EncryptionService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CORS", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            string jwtKey = builder.Configuration.GetSection("JWT").GetValue<string>("Secret") ?? "";

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            var app = builder.Build();

            app.UseCors("CORS");

            app.UseHttpsRedirection();

            app.UseAuthentication();            

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
