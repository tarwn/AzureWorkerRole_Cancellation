using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace WorkerRole1
{
	public class WorkerRole : RoleEntryPoint
	{
		private DateTime _startTime;

		public override void Run()
		{
			// This is a sample worker implementation. Replace with your logic.
			TraceWriteLine("Entry point called");


			while (true)
			{
				Thread.Sleep(10000);
				TraceWriteLine("Starting some work");
				DoWork();
				TraceWriteLine("Finished some work");
			}
		}

		public void DoWork()
		{
			Thread.Sleep(10000);
		}

		public override void OnStop()
		{
			TraceWriteLine("OnStop");
		}

		public override bool OnStart()
		{
			_startTime = DateTime.Now;
			// Set the maximum number of concurrent connections 
			ServicePointManager.DefaultConnectionLimit = 12;

			// For information on handling configuration changes
			// see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

			return base.OnStart();
		}

		public double SecondsSinceStarting
		{
			get
			{
				return (DateTime.Now - _startTime).TotalSeconds;
			}
		}

		public void TraceWriteLine(string message)
		{
			Trace.WriteLine(String.Format("{0:000.0}s - {1} - {2}", SecondsSinceStarting, "BasicWorker", message), "Information");
		}
	}
}
