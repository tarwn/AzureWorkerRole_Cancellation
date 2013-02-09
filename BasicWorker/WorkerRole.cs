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
		public override void Run()
		{
			// This is a sample worker implementation. Replace with your logic.
			Trace.WriteLine("BasicWorker: Entry point called", "Information");

			while (true)
			{
				Thread.Sleep(10000);
				Trace.WriteLine("BasicWorker: Starting some work", "Information");
				DoWork();
				Trace.WriteLine("BasicWorker: Finished some work", "Information");
			}
		}

		public void DoWork()
		{
			Thread.Sleep(10000);
		}

		public override void OnStop()
		{
			Trace.WriteLine("BasicWorker: OnStop", "Information");
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
