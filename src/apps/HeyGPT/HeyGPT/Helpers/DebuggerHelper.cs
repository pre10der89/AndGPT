using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HeyGPT.App.Helpers;
public static class DebuggerHelper
{
    /// <summary>
    /// Waits for a debugger to attach by showing a message box.
    /// This method is conditional on WAIT_FOR_DEBUGGER_TO_ATTACH and only executes in DEBUG builds.
    /// </summary>
    [Conditional("WAIT_FOR_DEBUGGER_TO_ATTACH")]
    public static void WaitForDebuggerToAttach()
    {
#if DEBUG
        if (!Debugger.IsAttached)
        {
            // Call the Win32 MessageBox function
            MessageBoxHelper.ShowMessageBox("Attach the debugger now and then click OK.", "Attach Debugger");
        }
#endif
    }
}
