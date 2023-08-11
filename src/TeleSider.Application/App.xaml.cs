using Backend;

namespace TeleSider;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
        Client.SetSessionPath(DeviceInfo.Current.Platform.ToString());
    }
}
