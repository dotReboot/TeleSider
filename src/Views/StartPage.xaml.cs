using System.Windows.Input;

namespace TeleSider.Views;	

public partial class StartPage : ContentPage
{
    public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

    public StartPage()
	{
		InitializeComponent();
		BindingContext = this;
	}
}