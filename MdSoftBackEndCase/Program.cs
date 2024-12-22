using DocumentManagementSystem.Data;
using MdSoftBackEndCase.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MdSoftBackEndCase
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"]
                    };
                });

            builder.Services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
            });

           
            builder.Services.AddScoped<IDocumentService, DocumentService>(); 

            builder.Services.AddOpenApi();

            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseCors();
            app.UseHttpsRedirection();
            app.UseAuthentication();  
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
