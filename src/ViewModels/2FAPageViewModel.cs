using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Numerics;
using TeleSider.Pages;

namespace TeleSider.ViewModels;

public partial class _2FAPageViewModel : ObservableObject
{

    [ObservableProperty]
    public string password = null;

    [RelayCommand]
    public async Task SubmitButtonPressed()
    {
        if (Password == null || Password.Length == 0)
        {
            await Shell.Current.DisplayAlert("Invalid password", "Please, try entering your password again", "Ok", "Cancel", FlowDirection.LeftToRight);
        }
    }
}