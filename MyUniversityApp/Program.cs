using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using System.Text;
using UniversityApp.Application.Interfaces;
using UniversityApp.Application.Services;
using UniversityApp.Domain.Entities;
using UniversityApp.Infrastructure.Authentication;
using UniversityApp.Infrastructure.Configuration;
using UniversityApp.Infrastructure.Data;
using UniversityApp.Infrastructure.Email;
using UniversityApp.Infrastructure.Repositories;
namespace MyUniversityApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.Configure<EmailSettings>(
                builder.Configuration.GetSection("EmailSettings"));

            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));
            var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>();

            if (jwtSettings is null)
            {
                throw new InvalidOperationException("JwtSettings-ը բացակայում է appsettings.json-ում։");
            }

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,

                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtSettings.Key)),

                            ClockSkew = TimeSpan.Zero
                        };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddScoped<ITokenService, TokenService>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddOpenApi();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Մուտքագրիր միայն JWT access token-ը"
                });

                options.AddSecurityRequirement(document =>
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                    });
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IStudentService, StudentService>();


            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddHttpClient();

            builder.Services.AddScoped(
    typeof(IGenericRepository<>),
    typeof(GenericRepository<>));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context =
                    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                Console.WriteLine($"Database: {context.Database.GetDbConnection().Database}");
                Console.WriteLine($"Server: {context.Database.GetDbConnection().DataSource}");
            }


            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllers();

            app.Run();
        }
    }
}
