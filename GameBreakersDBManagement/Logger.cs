using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GameBreakersDBManagement
{
    public static class Logger
    {
        //TODO: Use TraceSource?
        private static readonly object _syncObject = new object();

        private static readonly Dictionary<string, string> ValidActions = new Dictionary<string, string>
        {
            { "Fetch ID", "Error One" },
            { "Fetch Data", "Error Two" }
        };

        public static void LogActivity(string message, string extraData = "")
        {
            lock (_syncObject)
            {
                Dictionary<string, object> values = new Dictionary<string, object>
                {
                    { "Action", message },
                    { "TimeOfActivity", DateTime.Now.ToString() },
                    { "ExtraData", extraData },
                };

                var query = "INSERT INTO ActivityLog ";

                DatabaseManager.RunQueryWithArgs(query, values);
            }
        }

        public static void LogError(string attemptedAction, string error, string extraData)
        {
            lock (_syncObject)
            {
                if (error == "Thread was being aborted.")
                    return;

                Dictionary<string, object> values = new Dictionary<string, object>
                {
                    { "AttemptedAction", attemptedAction },
                    { "Error", error },
                    { "ExtraData", extraData },
                    { "TimeOfError", DateTime.Now.ToString() },
                    { "ParentFunction", new StackFrame(1).GetMethod().Name }
                };

                var query = "INSERT INTO Errors ";

                DatabaseManager.RunQueryWithArgs(query, values);
            }
        }
    }
}
