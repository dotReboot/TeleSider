using System.Diagnostics;
using System.IO.Enumeration;
using TL;

namespace TeleSider.Backend;
public static partial class Client
{
    private static WTelegram.Client? _client;
    private static Messages_MessagesBase? _savedMessagesHistory;

    public static TL.User User
    {
        get
        {
            if (_client is null || _client.User is null)
            {
                throw new NullReferenceException();
            }
            return _client.User;
        }
    }

    private static readonly int _apiID;
    private static readonly string _apiHash;
    private static string? _sessionPath;

    public static bool isLoggedIn = false;
    public static bool isExistingSessionChecked = false;



    public static async Task Login(string? phoneNumber=null)
    {
        DisposeClient();
        _client = new WTelegram.Client(Config);
        await DoLogin(phoneNumber, "verification_code");
    }

    // returns the required Item, null if the user is logged in
    public static async Task<string?> DoLogin(string? loginInfo, string? requiredItem)
    {
        while (_client.User is null)
            switch (await _client.Login(loginInfo))
            {
                case "verification_code": 
                {
                    if (requiredItem == "verification_code")
                        return requiredItem;
                    else
                        throw new Exception("Invalid Verification Code");
                }
                case "password": 
                {
                    if (requiredItem == "password")
                        return requiredItem;
                    else
                        throw new Exception("Invalid Password");
                }
                default:
                {
                    break;
                }
            }
        isLoggedIn = true;
        await DownloadSavedMessagesHistory();
        await DownloadUserAvatar(_client.User, "avatar.jpg");
        return null;
    }

    // returns true if the current user is successfully logged in, otherwise returns false
    public static async Task<bool> ResumeSession()
    {
        if (!isExistingSessionChecked)
        {
            CreateClientIfNeeded();
            try
            {
                await _client.LoginUserIfNeeded(null, false);
                isLoggedIn = true;
                await DownloadSavedMessagesHistory();
                await DownloadUserAvatar(_client.User, "avatar.jpg");

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to resume the session");
                Debug.WriteLine(ex.Message);
            }
            isExistingSessionChecked = true;
        }
        return isLoggedIn;
    }

    private static void CreateClientIfNeeded() => _client ??= new WTelegram.Client(Config);

    static string? Config(string item)
    {
        switch (item)
        {
            case "api_id": return _apiID.ToString();
            case "api_hash": return _apiHash;
            case "session_pathname": return _sessionPath;
            case "user_id": return "-1";
            default: return null;
        }
    }

    public static void DisposeClient() => _client?.Dispose();


    public static async Task DownloadSavedMessagesHistory()
    {
        _savedMessagesHistory = await _client.Messages_GetHistory(InputPeer.Self);
    }

    public static async Task DownloadUserAvatar(IPeerInfo peer, string filename)
    {
        try
        {
            using var fileStream = File.Create(Path.Combine(FileSystem.Current.CacheDirectory, filename));
            var type = await _client.DownloadProfilePhotoAsync(peer, fileStream);
            fileStream.Close();
        }
        catch (Exception e)
        {
#if DEBUG
            Debug.WriteLine(e);
#endif
        }
    }

    public static async Task<int> CountTextMessages()
    {
        if (_savedMessagesHistory is null)
        {
            await DownloadSavedMessagesHistory();
        }
        return _savedMessagesHistory.Messages.OfType<Message>().Where(m => !m.flags.HasFlag(TL.Message.Flags.has_media))
                                                 .Select(m => m.message).ToArray().Length;
    }

    public static async Task<int> CountMessagesOfMediaType<T>() where T : MessageMedia
    {
        if (_savedMessagesHistory is null)
        {
            await DownloadSavedMessagesHistory();
        }
        return _savedMessagesHistory.Messages.OfType<Message>().Select(m => m.media).OfType<T>().ToArray().Length;
    }



    public static void SetSessionPath(string platform)
    {
        switch (platform)
        {
            case "Android":
                _sessionPath = "/data/data/com.telesider.telesider/files/TeleSider.session";
                break;
            case "WinUI":
                break;
            // Other platforms
            default:
                throw new Exception("The platform is unknown");
        }
    }

    private static void DebugLogger(int level, string message) => Debug.WriteLine(message);
}
