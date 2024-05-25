using System.Diagnostics;

namespace AndGPT.UI.Core.Services;

// EventLog is mostly windows, so we put it in the UI.Core, which is a de facto windows container right now.

public class EventLogService
{
    public static void WriteInfoLog(string message)
    {
        WriteToApplicationEventLog(message, EventLogEntryType.Information);
    }

    public static void WriteErrorLog(string message)
    {
        WriteToApplicationEventLog(message, EventLogEntryType.Error);
    }

    private static void WriteToApplicationEventLog(string message, EventLogEntryType severity)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        try
        {
            using var eventLog = new EventLog("Application");

            eventLog.Source = "Application";
            eventLog.WriteEntry(message, severity);
        }
        catch
        {
            // Ignored
        }
    }
}
