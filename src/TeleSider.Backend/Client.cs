using System.Diagnostics;
using WTelegram;

namespace Backend;
public static partial class Client
{
    private static WTelegram.Client? _client;

    private static readonly int apiID;
    private static readonly string apiHash;
    private static string? _sessionPath;

    public static async Task Login(string? phoneNumber=null)
    {
        _client?.Dispose();
        CreateClientIfNeeded();
        await DoLogin(phoneNumber, "verification_code");
    }

    // returns the required Item, null if the user is logged in
    public static async Task<string?> DoLogin(string? loginInfo, string? requiredItem)
    {
        while (_client.User == null)
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
        return null;
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
    private static void CreateClientIfNeeded() => _client ??= new WTelegram.Client(apiID, apiHash, _sessionPath);
    public static string GetUsername()
    {
        if (_client != null)
        {
            return _client.User.ToString();
        }
        throw new Exception("You must login first");
    }
    private static void DebugLogger(int level, string message) => Debug.WriteLine(message);
}