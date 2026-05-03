using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.IdentityModel.Tokens;
using RoomReservationSystem.Web.Repositories;
using RoomReservationSystem.Web.Services;
using System.Text;


namespace RoomReservationSystem.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IUserRepository, UserRepository>(
                _ => new UserRepository(connectionString!));
            builder.Services.AddScoped<IRoomRepository, RoomRepository>(
                _ => new RoomRepository(connectionString!));
            builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>(
                _ => new EquipmentRepository(connectionString!));
            builder.Services.AddScoped<IRoomEquipmentRepository, RoomEquipmentRepository>(
                _ => new RoomEquipmentRepository(connectionString!));
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>(
                _ => new ReservationRepository(connectionString!));
            builder.Services.AddScoped<IReservationHistoryRepository, ReservationHistoryRepository>(
                _ => new ReservationHistoryRepository(connectionString!));

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IStatisticsService, StatisticsService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["jwt"];
                            return Task.CompletedTask;
                        }
                    };
                });

            var app = builder.Build();

            using var connection = new SqliteConnection(connectionString);
            var initScript = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "../Database/init.sql"));
            connection.Execute(initScript);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
