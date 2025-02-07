using Microsoft.EntityFrameworkCore;
using OurHeroWebAPI.Entities;
using OurHeroWebAPI.Services;

namespace OurHeroWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<OurHeroDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IOurHeroService, OurHeroService>();
            builder.Services.AddScoped<IOurHeroServiceDB, OurHeroServiceDB>();
            builder.Services.AddScoped<IOurHeroServiceADO, OurHeroServiceADO>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Map controller routes
            app.MapControllers();

            app.Run();
        }
    }
}
