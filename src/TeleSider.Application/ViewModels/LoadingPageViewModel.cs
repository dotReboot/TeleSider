using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TeleSider.Pages;
using TeleSider.Backend;

namespace TeleSider.ViewModels;

public partial class LoadingPageViewModel : ObservableObject
{
    [ObservableProperty]
    private Color pageColor;

    [ObservableProperty]
    private string pageColorText;

    [ObservableProperty]
    private string loadingPageText;

    [RelayCommand]
    private void Appearing()
    {
        SetBackgroundColor();
        LoadingPageText = Client.isExistingSessionChecked ? "Tap on the screen to navigate" : "Loading...";
    }

    [RelayCommand]
    private async Task ScreenTapped()
    {
        if (Client.isExistingSessionChecked)
        {
            if (Client.isLoggedIn)
            {
                await Shell.Current.GoToAsync(nameof(HomePage));
            }
            else
            {
                await Shell.Current.GoToAsync(nameof(StartPage));
            }
        }
    }

    [RelayCommand]
    private void SwipedDown()
    {
        App.Current.Quit();
    }
    [RelayCommand]
    private void SetBackgroundColor()
    {
        Random random = new();
        List<Color> colors = new();
        foreach (string colorName in new string[]{"Primary", "Secondary", "Quaternary", "Quinary"})
        {
            if (App.Current.Resources.TryGetValue(colorName, out var color))
            {
                colors.Add((Color)color);
            }
        }
        if (!colors.Any())
        {
            throw new Exception("No colors defined for the application");
        }
        else
        {
            PageColor = colors[random.Next(colors.Count)];
        }
        PageColorText = PageColor.ToHex();
        CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(PageColor);
    }
}