using MonkeyFinder.View;
using Microsoft.Extensions.Logging;
using MonkeyFinder.Services;

namespace MonkeyFinder;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
        builder.Services.AddTransient<MonkeyService>();
        builder.Services.AddTransient<MonkeysViewModel>();

#if DEBUG
        builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endif

        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
    }
}
