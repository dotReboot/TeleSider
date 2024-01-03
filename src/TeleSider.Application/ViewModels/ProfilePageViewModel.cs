using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TeleSider.Backend;
using TL;

namespace TeleSider.ViewModels;

public partial class ProfilePageViewModel : ObservableObject
{
    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string firstName;

    [ObservableProperty]
    private string lastName;

    [ObservableProperty]
    private string phoneNumber;

    [ObservableProperty]
    private ImageSource profilePhotoPath;

    [ObservableProperty]
    private string badgeText;

    [ObservableProperty]
    private int photoCount;

    [ObservableProperty]
    private int documentCount;

    [ObservableProperty]
    private int noteCount;

    [ObservableProperty]
    private string sizeLimit;

    [RelayCommand]
    private async Task Appearing()
    {
        User user = Client.User;
        Username = $"@{user.username}";
        FirstName = user.first_name;
        if (user.last_name != null)
        {
            LastName = user.last_name;
        }
        PhoneNumber = $"+{user.phone}";
        ProfilePhotoPath = ImageSource.FromFile(Path.Combine(FileSystem.Current.CacheDirectory, "avatar.jpg"));
        BadgeText = "<Code Surfer>";
        SizeLimit = user.flags.HasFlag(TL.User.Flags.premium) ? "4GB" : "2GB";
        PhotoCount = await Client.CountMessagesOfMediaType<MessageMediaPhoto>();
        DocumentCount = await Client.CountMessagesOfMediaType<MessageMediaDocument>();
        NoteCount = await Client.CountTextMessages();
    }
}