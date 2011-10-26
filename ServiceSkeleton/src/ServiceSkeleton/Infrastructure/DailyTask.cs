namespace ServiceSkeleton.Infrastructure
{
	using System;
	using System.Collections.Generic;
	using BclExtensionMethods.Scheduling.Simple;
	using NServiceBus;
	using Status;
	using log4net;

	public abstract class DailyTask : IWantToRunAtStartup, IHaveATaskMonitor, IHaveStatusToReport
	{
		private readonly bool _Enabled;
		private readonly Daily _Timer;
		protected readonly ILog Log = LogManager.GetLogger(typeof (DailyTask).FullName);

		public DailyTask(bool enabled, params TimeSpan[] timesOfDay)
		{
			_Enabled = enabled;
			_Timer = new Daily(ExecuteWithLog, timesOfDay);
			TaskMonitor = new TaskMonitor();
		}

		public TaskMonitor TaskMonitor { get; private set; }

		private void ExecuteWithLog()
		{
			if (!_Enabled)
			{
				return;
			}
			Log.Debug("Triggering " + GetType());
			try
			{
				Execute();
			}
			catch (Exception exception)
			{
				Log.Error("Daily execution failed", exception);
			}
		}

		public abstract void Execute();

		public void Run()
		{
			_Timer.Start();
		}

		public void Stop()
		{
			_Timer.Stop();
		}

		public abstract IEnumerable<object> MyStatuses();
	}
}