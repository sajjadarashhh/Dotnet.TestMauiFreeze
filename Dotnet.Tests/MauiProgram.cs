using Blazored.LocalStorage;
using Dotnet.Tests.Data;

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

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSingleton<StateManager>(a => new StateManager(a.GetRequiredService<ILocalStorageService>()));

            return builder.Build();
        }
    }
}
public class StateBaseExtensions
{
    //you can see Freeze Problem and Null refrence Exception within this variable
    public static bool IsFreezeException = false;
    //we can solve problem with some hacks but this is not Clean and true to use in our Applications written in WASM we cant to switch all configurations to this...
    public static bool HandleProblems = false;
}
public class StateManager
{
    public string UserState { get; set; }
    public StateManager(ILocalStorageService localStorageService)
    {
        if (!StateBaseExtensions.IsFreezeException)
            UserState = localStorageService.GetItemAsStringAsync("User").Result;//we need to get items in constructor! we can to get them in another way but best practices has exception and we cant use best architectures
    }
    public StateManager()
    {

    }
    public async Task Initialize(ILocalStorageService localStorageService)
    {
        UserState = await localStorageService.GetItemAsStringAsync("User");
    }
}