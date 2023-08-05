﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TeleSider.Pages;
using BackEnd;

namespace TeleSider.ViewModels;

public partial class StartPageViewModel : ObservableObject
{

    [ObservableProperty]
    private string phoneNumber = null;

    [RelayCommand]
    private async Task TapSignUp(string url) => await Launcher.OpenAsync(url);

    [RelayCommand]
    private async Task SignInButtonPressed()
    {
#if ANDROID
        Platforms.KeyboardManager.HideKeyboard();
#endif
        if (!String.IsNullOrWhiteSpace(PhoneNumber))
        {
            PhoneNumber = PhoneNumber.Replace(" ", "");
            if (PhoneNumber.Length >= 7)
            {
                if (PhoneNumber.All(Char.IsDigit))
                {
                    bool request = await Shell.Current.DisplayAlert("Is this the correct number?", $"+{PhoneNumber}", "Yes", "Edit", FlowDirection.LeftToRight);
                    if (request)
                    {
                        // Asking the user to grant the required permission, if they won't - close the app
                        await PermissionManager.CheckAndRequestReadWrite();
                        try
                        {
                            await Client.Login($"+{PhoneNumber}");
                            await NavigateToNumberVerificationPage();
                        }
                        catch
                        {
                            await DisplayInvalidPhoneNumberAlert("Something went wrong while trying to sign you in. Please, try again");
                            // remove a session 
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
                await DisplayInvalidPhoneNumberAlert("Please, make sure the phone number is correct and try again");
            }
        }
        else
        {
            await DisplayInvalidPhoneNumberAlert("Please, fill in all required fields");
        }
    }
    // %2b means "+" in url, it is the only way to pass the phone number with a "+" sign
    [RelayCommand]
    private async Task NavigateToNumberVerificationPage() => await Shell.Current.GoToAsync($"{nameof(PhoneVerificationPage)}?PhoneNumber=%2b{PhoneNumber}");

    private async Task DisplayInvalidPhoneNumberAlert(string details)
    {
        await Shell.Current.DisplayAlert("Invalid phone number", details, "Ok", "Cancel", FlowDirection.LeftToRight);
    }
}