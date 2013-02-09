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

		public override void Run()
		{
			Trace.WriteLine("SafeWorker: Entry point called", "Information");
			_cancellationTokenSource = new CancellationTokenSource();
			_safeToExitHandle = new ManualResetEvent(false);
			var token = _cancellationTokenSource.Token;

			while (!token.IsCancellationRequested)
			{
				Trace.WriteLine("SafeWorker: Starting some work", "Information");
				DoWork();
				Trace.WriteLine("SafeWorker: Finished some work", "Information");
				token.WaitHandle.WaitOne(10000);	// sleep 10s or exit early if cancellation is signalled
			}

			Trace.WriteLine("SafeWorker: Ready to exit", "Information");
			_safeToExitHandle.Set();	// cleanly exited the main loop
		}

		public void DoWork()
		{
			Thread.Sleep(10000);
		}

		public override void OnStop()
		{
			Trace.WriteLine("SafeWorker: OnStop Called", "Information");
			_cancellationTokenSource.Cancel();
			_safeToExitHandle.WaitOne();
			Trace.WriteLine("SafeWorker: OnStop Complete, Exiting Safely", "Information");
		}

		public override bool OnStart()
		{
			// Set the maximum number of concurrent connections 
			ServicePointManager.DefaultConnectionLimit = 12;

			// For information on handling configuration changes
			// see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

			return base.OnStart();
		}
	}
}
