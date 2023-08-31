using Microsoft.EntityFrameworkCore;
using PRS_GM.Data;

namespace PRS_GM {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DevDb") ?? throw new InvalidOperationException("Connection string not found.")));


            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddCors();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}