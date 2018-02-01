using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBreakersDBManagement
{
    class Logger
    {
        //TODO: Use TraceSource?
        private static readonly object _syncObject = new object();

        static Logger Log = null;
        static string ActivityLog = @"C:\GameBreakersInventory\Logging\ActivityLog.txt";
        static string ErrorLog = @"C:\GameBreakersInventory\Logging\ErrorLog.txt";

        public static Logger GetLogger()
        {
            if (Log == null)
            {
                Log = new Logger();
            }

            return Log;
        }

        Logger()
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

        public void LogActivity(string message)
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

        public void LogError(string message)
        {
            //TIME | ERROR
            lock (_syncObject)
            {
                message = "\r\n----    ----    ----    ----\r\n" + DateTime.Now.ToString() + "\r\n" + message;
                StreamWriter file = new StreamWriter(ErrorLog, true);
                file.WriteLine(message);

                file.Close();
            }
        }
    }
}
