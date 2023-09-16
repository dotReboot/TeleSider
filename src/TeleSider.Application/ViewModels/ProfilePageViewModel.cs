using CommunityToolkit.Mvvm.ComponentModel;
using Backend;

namespace TeleSider.ViewModels;

public partial class ProfilePageViewModel : ObservableObject
{
    [ObservableProperty]
    private string username = Client.GetUsername();
}