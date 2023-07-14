using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace TeleSider.ViewModels;
public partial class StartPageViewModel : ObservableObject
{
    [ObservableProperty]
    public string fullPhoneNumber = null;

    [ObservableProperty]
    public string countryCode = null;

    [ObservableProperty]
    public string phoneNumber = null;

    [RelayCommand]
    public async Task TapSignUp(string url) => await Launcher.OpenAsync(url);

    [RelayCommand]
    public async Task SignInButtonPressed()
    {
        if (CountryCode != null && PhoneNumber != null) {
            FullPhoneNumber = CountryCode + PhoneNumber;
            FullPhoneNumber = FullPhoneNumber.Replace(" ", "");
            if (FullPhoneNumber.All(Char.IsDigit))
            {
                FullPhoneNumber = '+' + FullPhoneNumber;
                bool request = await Shell.Current.DisplayAlert("Is this the correct number?", FullPhoneNumber, "Yes", "Edit", FlowDirection.LeftToRight);
                if (request)
                {
                    try
                    {
                        await NavigateToNumberVerificationPage();
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Sorry, the page you are navigating to was not created yet");
                    }
                }
            }
            else
            {
                await DisplayInvalidPhoneNumberAlert("Please, try again");
            }
        }
        else
        {
            await DisplayInvalidPhoneNumberAlert("Please, fill in all required fields");
        }
    }

    [RelayCommand]
    public Task NavigateToNumberVerificationPage() => throw new NotImplementedException();

    private async Task DisplayInvalidPhoneNumberAlert(string details)
    {
        await Shell.Current.DisplayAlert("Invalid phone number", details, "Ok", "Cancel", FlowDirection.LeftToRight);
    }
}