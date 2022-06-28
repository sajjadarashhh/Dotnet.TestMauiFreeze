using Blazored.LocalStorage;
using Dotnet.Tests.Data;
using Microsoft.JSInterop;

namespace Dotnet.Tests
{

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

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif
            var result =  builder.Build();
            result.Services.GetRequiredService<IJSRuntime>().InvokeVoidAsync("window.location.reload").ConfigureAwait(false).GetAwaiter().GetResult();
            return result;
        }
    }
}