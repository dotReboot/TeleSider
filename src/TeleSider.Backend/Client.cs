using System.Diagnostics;
using WTelegram;

namespace Backend;
public static class Client
{
    private static WTelegram.Client? client;
    private static string? _sessionPath;
    public static string username = "";
    private static readonly int apiID = int.Parse(Environment.GetEnvironmentVariable("apiID"));
    private static readonly string apiHash = Environment.GetEnvironmentVariable("apiHash");

    static Client()
    {
        if (apiID == 1 || apiHash == null)
        {
            throw new Exception("Invalid apiID or apiHash");
        }
        Helpers.Log = DebugLogger;
    }

    public static async Task Login(string? phoneNumber=null)
    {
        client?.Dispose();
        CreateClientIfNeeded();
        await DoLogin(phoneNumber, "verification_code");
    }

    // returns the required Item, null if the user is logged in
    public static async Task<string?> DoLogin(string? loginInfo, string? requiredItem)
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
                default:
                {
                    break;
                }
            }
        username = client.User.ToString();
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
    private static void CreateClientIfNeeded()
    {
        client ??= new WTelegram.Client(apiID, apiHash, _sessionPath);
    }
    private static void DebugLogger(int level, string message) => Debug.WriteLine(message);
}