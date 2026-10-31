using Microsoft.EntityFrameworkCore;
using VinylStore.Core.Interfaces;
using VinylStore.Infrastructure.Data;
using VinylStore.Infrastructure.Mappings;
using VinylStore.Infrastructure.Repositories;
using VinylStore.Services.Interfaces;
using VinylStore.Services.Services;
using VinylStore.Services.Validators;
using VinylStore.Api.Filters;

namespace VinylStore.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configurar la BD MySql
            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
            builder.Services.AddDbContext<VinylStoreContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            #endregion

            // Add services to the container.
            builder.Services.AddScoped(typeof(IBaseRepository<>),typeof(BaseRepository<>));

            // IServices y Services
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IArtistService, ArtistService>();
            builder.Services.AddScoped<IGenreService, GenreService>();
            builder.Services.AddScoped<IAlbumService, AlbumService>();
            builder.Services.AddScoped<IVinylService, VinylService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            // builder.Services.AddControllers();
            builder.Services.AddControllers(options =>
            {
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Automapper
            builder.Services.AddAutoMapper(typeof(CustomerProfile).Assembly);

            // Validators
            builder.Services.AddScoped<CustomerDtoValidator>();
            builder.Services.AddScoped<CrearCustomerDtoValidator>();
            builder.Services.AddScoped<ActualizarCustomerDtoValidator>();
            
            builder.Services.AddScoped<ArtistDtoValidator>();
            builder.Services.AddScoped<CrearArtistDtoValidator>();
            builder.Services.AddScoped<ActualizarArtistDtoValidator>();

            builder.Services.AddScoped<GenreDtoValidator>();
            builder.Services.AddScoped<CrearGenreDtoValidator>();
            builder.Services.AddScoped<ActualizarGenreDtoValidator>();

            builder.Services.AddScoped<AlbumDtoValidator>();
            builder.Services.AddScoped<CrearAlbumDtoValidator>();
            builder.Services.AddScoped<ActualizarAlbumDtoValidator>();

            builder.Services.AddScoped<VinylDtoValidator>();
            builder.Services.AddScoped<CrearVinylDtoValidator>();
            builder.Services.AddScoped<ActualizarVinylDtoValidator>();

            builder.Services.AddScoped<EmployeeDtoValidator>();
            builder.Services.AddScoped<CrearEmployeeDtoValidator>();
            builder.Services.AddScoped<ActualizarEmployeeDtoValidator>();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }
    }
}