using System.Diagnostics;

namespace TeleSider;
public static class ShellNavigation
{
    public static void ClearNavigationStack()
    {
        try
        {
            var stack = Shell.Current.Navigation.NavigationStack.ToArray();
            for (int i = stack.Length - 1; i > 0; i--)
            {
                Shell.Current.Navigation.RemovePage(stack[i]);
            }
        }
        catch (Exception ex)
        {
#if DEBUG
            Debug.WriteLine("The Navigation Stack was not cleared. An exception occurred");
            Debug.WriteLine(ex);
#endif
        }
    }
}