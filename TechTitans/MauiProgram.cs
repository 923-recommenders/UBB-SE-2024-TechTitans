using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TechTitans.Repositories;
using System.Reflection;
using CommunityToolkit.Maui;
using TechTitans.Views;
using TechTitans.Views.Components;

namespace TechTitans
{
    public static class MauiProgram
    {
        public static IConfiguration Configuration { get; private set; }
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            //builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<UserPage>();
            builder.Services.AddScoped<BackHomeButton>();
            builder.Services.AddScoped<ArtistPage>();
            builder.Services.AddScoped<MainPage>();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


#if DEBUG
            builder.Logging.AddDebug();
#endif
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TechTitans.appsettings.json");
            builder.Configuration.AddJsonStream(stream).Build();
            var app = builder.Build();
            Configuration = app.Services.GetService<IConfiguration>();
            return app;
        }
    }
}
