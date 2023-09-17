using TeleSider.ViewModels;

namespace TeleSider.Pages;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(ProfilePageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}