namespace ServiceSkeleton.Infrastructure.Status
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using BclExtensionMethods.Concurrency;
	using Samples.Status;

	public class TaskMonitor
	{
		private readonly ConcurrentCollection<Task> _Tasks = new ConcurrentCollection<Task>();

		/// <summary>
		/// 	Start an action as a task and monitor it, returning the task upon starting
		/// </summary>
		public Task StartTask<T>(Action<T> action, T progress)
			where T : AsyncProgress
		{
			var task = new Task(o => action(o as T), progress);
			return StartTask(task);
		}

		/// <summary>
		/// 	Start a task and monitor it, returning the task upon starting
		/// </summary>
		public Task StartTask(Task task)
		{
			_Tasks.TryAdd(task);
			task.ContinueWith(t => _Tasks.TryRemove(t));
			task.Start();
			return task;
		}

		/// <summary>
		/// 	Monitor a task (running or not)
		/// </summary>
		public void MonitorTask(Task task)
		{
			task.ContinueWith(t => _Tasks.TryRemove(t));
			_Tasks.TryAdd(task);
			if (task.IsCompleted)
			{
				_Tasks.TryRemove(task);
			}
		}

		/// <summary>
		/// 	Get the progress of the tasks being monitored
		/// </summary>
		public IEnumerable<AsyncTaskProgress> GetProgress()
		{
			return _Tasks
				.Select(t => new AsyncTaskProgress(t))
				.ToList();
		}
	}
}