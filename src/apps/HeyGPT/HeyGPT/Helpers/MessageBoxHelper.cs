using System.Runtime.InteropServices;

namespace HeyGPT.App.Helpers;
public static class MessageBoxHelper
{
    // P/Invoke signature for the Win32 MessageBox function
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    // Constants for message box buttons and icon types
    private const uint MB_OK = 0x00000000;
    private const uint MB_ICONINFORMATION = 0x00000040;

    /// <summary>
    /// Shows a Win32 message box with the specified message.
    /// </summary>
    /// <param name="message">The message to display in the message box.</param>
    public static void ShowMessageBox(string message, string v)
    {
        // Call the Win32 MessageBox function
        MessageBox(IntPtr.Zero, message, "Information", MB_OK | MB_ICONINFORMATION);
    }
}
