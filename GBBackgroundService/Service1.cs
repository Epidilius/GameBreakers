using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GBBackgroundService
{
    public partial class Service1 : ServiceBase
    {
        //TODO: Make the DB Manager a service? Make this a thread?
        EventLog Logger;
        int CurrentIteration;
        string ConnectionString;

        public Service1()
        {
            InitializeComponent();
            Logger = new EventLog();
            if (!EventLog.SourceExists("GB_MtG_Source"))
            {
                EventLog.CreateEventSource("GB_MtG_Source", "GB_MtG_Log");
            }
            Logger.Source = "GB_MtG_Source";
            Logger.Log = "GB_MtG_Log";

            CurrentIteration = 0;

            ConnectionString = ConfigurationManager.ConnectionStrings["GameBreakers"].ConnectionString; 

            Logger.WriteEntry("GameBreakers MtG service has been created");
        }

        static void Main()
        {
            ServiceBase.Run(new Service1());
        }

        protected override void OnStart(string[] args)
        {
            SetupTimer();
            Logger.WriteEntry("GameBreakers MtG service has started");
        }

        protected override void OnStop()
        {
            Logger.WriteEntry("GameBreakers MtG service has stopped. Performed " + CurrentIteration.ToString() + " database checks.");
        }
        
        void CheckDatabase(object sender, System.Timers.ElapsedEventArgs e)
        {
            CurrentIteration++;
            Logger.WriteEntry("Beginning database check #" + CurrentIteration.ToString());
        }

        void SetupTimer()
        {
            // Set up a timer to trigger every hour.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000 * 60 * 60;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.CheckDatabase);
            timer.Start();
        }
    }
}
