using Blazored.LocalStorage;
using Dotnet.Tests.Data;
using Pooyan.test.project.StateManagement;
using Pooyan.test.project.StateManagement.StateManagerExtensions;

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
            //you can see Freeze Problem and Null refrence Exception within this variable
            StateBaseExtensions.IsFreezeException = true;
            //we can solve problem with some hacks but this is not Clean and true to use in our Applications written in WASM we cant to switch all configurations to this...
            StateBaseExtensions.HandleProblems = false;
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddSingleton<WeatherForecastService>();
            if (!StateBaseExtensions.IsFreezeException)
                StateContext.ConfigurationNullRefrenceExceptions(builder.Services.BuildServiceProvider());
            builder.Services.AddSingleton<StateContext>();

            return builder.Build();
        }
    }
}