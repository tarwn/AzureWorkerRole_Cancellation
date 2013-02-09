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

namespace SafeWorkerRole
{
	public class WorkerRole : RoleEntryPoint
	{
		private CancellationTokenSource _cancellationTokenSource;
		private ManualResetEvent _safeToExitHandle;
		private DateTime _startTime;

		public override void Run()
		{
			TraceWriteLine("Entry point called");
			_cancellationTokenSource = new CancellationTokenSource();
			_safeToExitHandle = new ManualResetEvent(false);
			var token = _cancellationTokenSource.Token;

			while (!token.IsCancellationRequested)
			{
				TraceWriteLine("Starting some work");
				DoWork();
				TraceWriteLine("Finished some work");
				token.WaitHandle.WaitOne(10000);	// sleep 10s or exit early if cancellation is signalled
			}

			TraceWriteLine("Ready to exit");
			_safeToExitHandle.Set();	// cleanly exited the main loop
		}

		public void DoWork()
		{
			Thread.Sleep(10000);
		}

		public override void OnStop()
		{
			TraceWriteLine("OnStop Called");
			_cancellationTokenSource.Cancel();
			_safeToExitHandle.WaitOne();
			TraceWriteLine("OnStop Complete, Exiting Safely");
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
			Trace.WriteLine(String.Format("{0:000.0}s - {1} - {2}", SecondsSinceStarting, "SafeWorker", message), "Information");
		}
	}
}
