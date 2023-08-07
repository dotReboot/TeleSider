using System.Diagnostics;
using WTelegram;

namespace BackEnd;
public static class Client
{
    private static WTelegram.Client? client = null;
    public static string? platform = null;
    private static string? _sessionPath;
    public static string username = "";
    private static readonly int apiID = 1;
    private static readonly string? apiHash = null;

    static Client()
    {
        Helpers.Log = DebugLogger;
    }

    public static async Task Login(string phoneNumber)
    {
        if (client != null)
        {
            client.Dispose();
        }
        else
        {
            SetSessionPath();
        }
        if (apiID == 1 || apiHash == null)
        {
            throw new Exception("Invalid apiID or apiHash");
        }
        client = new WTelegram.Client(apiID, apiHash, _sessionPath);
        await DoLogin(phoneNumber, "verification_code");
    }

    public static async Task<string> DoLogin(string loginInfo, string? requiredItem)
    {
        while (client.User == null)
            switch (await client.Login(loginInfo))
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
                default: loginInfo = null; break;
            }
        username = client.User.ToString();
        return username;
    }
    private static void SetSessionPath()
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
                throw new Exception("Platform is unknown");
        }
    }

    private static void DebugLogger(int level, string message) => Debug.WriteLine(message);
}