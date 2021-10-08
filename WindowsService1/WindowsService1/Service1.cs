using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        Timer m_serviceTimer;

        public Service1()
        {
            InitializeComponent();

        }

        protected override void OnStart(string[] args)
        {
            WriteTextToLogFile("Service On-Start " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            m_serviceTimer = new Timer(1000);
            m_serviceTimer.Elapsed += new ElapsedEventHandler(HandleServiceTimer_Elapsed);
            m_serviceTimer.Enabled = true;

        }

        protected override void OnStop()
        {
            WriteTextToLogFile("Service on-Stop " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
        }
        public void WorkerProcess()
        {

            try
            {
                WriteTextToLogFile("In WorkerProcess");
            }
            catch (Exception ex)
            {
                WriteTextToLogFile("WorkerProcess: " + ex.Message);
                
            }

        }
        private void HandleServiceTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            System.Threading.Thread thread;

            try
            {
                m_serviceTimer.Enabled = false;

                thread = new System.Threading.Thread(WorkerProcess);

                thread.SetApartmentState(System.Threading.ApartmentState.STA);
                thread.Start();
                thread.Join();
            }
            catch (Exception exception)
            {
            }
            finally
            {
                m_serviceTimer.Enabled = true;
            }
        }


        private void WriteTextToLogFile(string textToWrite)
        {
            try
            {
                StreamWriter SW;
                string LogfileName = "ServiceLog_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
                SW = System.IO.File.AppendText(@"C:\ServiceLogs\" + LogfileName);
                // SW = System.IO.File.AppendText(@"C:\VideoService\WinServiceLog.txt");
                SW.WriteLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "-->>" + textToWrite);
                SW.Close();
            }
            catch
            {
            }
        }

    }
}
