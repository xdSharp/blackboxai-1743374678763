using Microsoft.Extensions.Logging;
using SalesApp.Pages;
using SalesApp.Services;
using SalesApp.ViewModels;

namespace SalesApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
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

        // Register Services
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<DataService>();

        // Register ViewModels
        builder.Services.AddTransient<AuthViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<SalesEntryViewModel>();
        builder.Services.AddTransient<ChartsViewModel>();

        // Register Pages
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<SalesEntryPage>();
        builder.Services.AddTransient<ChartsPage>();

        return builder.Build();
    }
}
