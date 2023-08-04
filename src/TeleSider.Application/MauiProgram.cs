using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using TeleSider.Pages;
using TeleSider.ViewModels;
using BackEnd;

namespace TeleSider;

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
        
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<HomePageViewModel>();

        builder.Services.AddScoped<StartPage>();
        builder.Services.AddScoped<PhoneVerificationPage>();
        builder.Services.AddScoped<_2FAPage>();
        builder.Services.AddScoped<StartPageViewModel>();
        builder.Services.AddScoped<PhoneVerificationPageViewModel>();
        builder.Services.AddScoped<_2FAPageViewModel>();


#if DEBUG
        builder.Logging.AddDebug();
#endif
        SetPlatform();
		return builder.Build();
	}
    private static void SetPlatform()
    {
        Client.platform = DeviceInfo.Current.Platform.ToString();
    }
}
