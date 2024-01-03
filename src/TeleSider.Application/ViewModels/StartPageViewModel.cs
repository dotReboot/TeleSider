using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TeleSider.Pages;
using TeleSider.Services;
using TeleSider.Backend;

namespace TeleSider.ViewModels;

public partial class StartPageViewModel : ObservableObject
{
    [ObservableProperty]
    private string signInButtonText = "Sign In";

    [ObservableProperty]
    private string phoneNumber = null;

    [ObservableProperty]
    private bool isSignInButtonEnabled = true;

    [RelayCommand]
    private async Task TapSignUp(string url) => await Launcher.OpenAsync(url);

    [RelayCommand]
    private async Task SignInButtonPressed()
    {
#if ANDROID
        KeyboardManager.HideKeyboard();
#endif
        if (String.IsNullOrWhiteSpace(PhoneNumber))
        {
            await DisplayInvalidPhoneNumberAlert("Please, fill in all required fields");
            return;
        }
        PhoneNumber = PhoneNumber.Replace(" ", "");
        if (PhoneNumber.Length < 7)
        {
            await DisplayInvalidPhoneNumberAlert("Please, make sure the phone number is correct and try again");
            return;
        }
        if (!PhoneNumber.All(Char.IsDigit))
        {
            await DisplayInvalidPhoneNumberAlert("Please, try again");
            return;
        }
        bool request = await Shell.Current.DisplayAlert("Is this the correct number?", $"+{PhoneNumber}", "Yes", "Edit", FlowDirection.LeftToRight);
        if (!request)
        {
            SetSignInButtonValues();
            return;
        }
        await PermissionManager.CheckAndRequestReadWrite();
        if (await ConnectionManager.IsConnected())
        {
            try
            {
                SetSignInButtonValues(true);
                await Client.Login($"+{PhoneNumber}");
                await NavigateToNumberVerificationPage();
            }
            catch (Exception ex)
            {
#if DEBUG
            Debug.WriteLine(ex);
#endif
            await Shell.Current.DisplayAlert("Something went wrong while trying to sign you in",
                                            "Make sure the phone number is correct, check your connection and try again",
                                            "Ok", FlowDirection.LeftToRight);
            }
            finally
            {
                SetSignInButtonValues();
            }
        }
    }

    // %2b means "+" in url, it is the only way to pass the phone number with a "+" sign
    private async Task NavigateToNumberVerificationPage() => await Shell.Current.GoToAsync($"{nameof(PhoneVerificationPage)}?PhoneNumber=%2b{PhoneNumber}");

    private async Task DisplayInvalidPhoneNumberAlert(string details)
    {
        await Shell.Current.DisplayAlert("Invalid phone number", details, "Ok", FlowDirection.LeftToRight);
    }
    private void SetSignInButtonValues(bool isloading=false)
    {
        SignInButtonText = isloading ? "Loading..." : "Sign In";
        IsSignInButtonEnabled = !isloading;
    }
}