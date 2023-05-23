using AwesomeDevEvents.API.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEvents.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DevEventsCs");

            //ADICIONANDO SINGLETON PARA PERSISTENCIA DE DADOS EM MEMORIA
            //builder.Services.AddDbContext<DevEventsDbContext>(db => db.UseInMemoryDatabase("DevEventsDb"));

            builder.Services.AddDbContext<DevEventsDbContext>(db => db.UseSqlServer(connectionString));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}