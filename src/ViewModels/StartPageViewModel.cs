using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace TeleSider.ViewModels;
public partial class StartPageViewModel : ObservableObject
{
    [RelayCommand]
    public async Task TapSignUp(string url) => await Launcher.OpenAsync(url);
}