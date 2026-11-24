using WebFilmes.Repositories;
using WebFilmes.Services;
using Microsoft.EntityFrameworkCore;
using WebFilmes.Data;
namespace WebFilmes
{
    public class Program 
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<FilmeService>();

            builder.Services.AddScoped<ImagemService>();

            //builder.Services.AddScoped<ResultadoServico>();

            builder.Services.AddScoped<IFilmeRepository, FilmeRepository>();

            var connectionString = builder.Configuration
            .GetConnectionString("FilmesConnection")
            ?? throw new InvalidOperationException(
                "A conexão FilmesConnection não foi configurada.");

            builder.Services.AddDbContext<FilmesContext>(options =>
                options.UseSqlServer(connectionString));

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
                pattern: "{controller=Filme}/{action=Index}/{id?}");

            app.Run();

        }
    }

}

