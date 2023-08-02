using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TeleSider.Pages;
using BackEnd;

namespace TeleSider.ViewModels;

[QueryProperty(nameof(PhoneNumber), "PhoneNumber")]
public partial class PhoneVerificationPageViewModel : ObservableObject
{
    [ObservableProperty]
    string phoneNumber;

    [ObservableProperty]
    public string verificationCode = null;

    [RelayCommand]
    public async Task SubmitButtonPressed()
    {
#if ANDROID
        Platforms.KeyboardManager.HideKeyboard();
#endif
        if (VerificationCode == null || VerificationCode.Length < 5 || !VerificationCode.All(Char.IsDigit))
        {
            await Shell.Current.DisplayAlert("Invalid verification code", "Please, check the verification code and try again", "Ok", "Cancel", FlowDirection.LeftToRight);
        }
        else
        {
            try
            {
                await Client.DoLogin(VerificationCode, "password");
                await Shell.Current.GoToAsync(nameof(_2FAPage));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Shell.Current.DisplayAlert("Invalid verification code", "Please, check the verification code and try again", "Ok", "Cancel", FlowDirection.LeftToRight);
            }
        }
    }
}