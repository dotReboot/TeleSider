﻿using System.Diagnostics;
using WTelegram;

namespace BackEnd;
public static class Client
{
    private static WTelegram.Client? client = null;
    public static string? platform = null;
    private static string? _sessionPath;
    public static string username = "";
    static Client ()
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
        client = new WTelegram.Client(1, "1", _sessionPath);
        await DoLogin(phoneNumber);
    }

    public static async Task DoLogin(string loginInfo)
    {
        while (client.User == null)
            switch (await client.Login(loginInfo))
            {
                case "verification_code": return;
                case "password": return;
                default: loginInfo = null; break;
            }
        username = client.User.ToString();
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