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
        //TODO: Push to database
        private static readonly object _syncObject = new object();
        static string ActivityLog = @"C:\GameBreakersInventory\Logging\ActivityLog.txt";
        static string ErrorLog = @"C:\GameBreakersInventory\Logging\ErrorLog.txt";

        private static readonly Dictionary<string, string> ValidActions = new Dictionary<string, string>
        {
            { "Fetch ID", "Error One" },
            { "Fetch Data", "Error Two" }
        };

        public static void Prep()
        {
            //TODO: Create files if they don't exist
            if (!File.Exists(ActivityLog))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ActivityLog));
                File.Create(ActivityLog);
            }
            if (!File.Exists(ErrorLog))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ErrorLog));
                File.Create(ErrorLog);
            }
        }

        public static void LogActivity(string message)
        {
            //TODO: Formatting
            //TIME | ACTIVITY 
            //01-09-2018 | Get price for Ixalan's Binding 
            //That? Should I have the result as well?
            lock (_syncObject)
            {
                message = "\r\n----    ----    ----    ----\r\n" + DateTime.Now.ToString() + "\r\n" +  message;
                StreamWriter file = new StreamWriter(ActivityLog, true);
                file.WriteLine(message);

                file.Close();
            }
        }

        public static void LogError(string attemptedAction, string error, string extraData)
        {
            lock (_syncObject)
            {
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
