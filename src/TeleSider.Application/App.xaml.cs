using Backend;

namespace TeleSider;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
        SetPlatform();
    }

    private static void SetPlatform()
    {
        Client.platform = DeviceInfo.Current.Platform.ToString();
    }
}
