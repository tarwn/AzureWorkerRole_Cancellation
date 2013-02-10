AzureWorkerRole Cancellation
============================

Blog Post: [Azure Worker Role - Exiting Safely, LessThanDot.com] (http://blogs.lessthandot.com/index.php/DesktopDev/MSTech/azure-worker-role-exiting-safely)

This project is a pair of Azure Worker Roles to show the difference in behavior 
between the out-of-the-box sample code Microsoft provides for a Worker Role and 
one that has been written to exit safely.

The BasicWorker is what Visual Studio provides when you create a new worker role 
project. It has a while(true) run loop that uses a thread.sleep call to sleep in 
between work. I added additional output, a fake DoWork call to simulate some work-
load and an OnStop override to log when the stop gets called.

The SafeWorker uses a cancellation token and "ready to exit" token to ensure that
in progress work is completed safely before the worker exits.

