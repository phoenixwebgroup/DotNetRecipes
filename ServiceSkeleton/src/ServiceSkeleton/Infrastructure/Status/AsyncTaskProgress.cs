namespace ServiceSkeleton.Infrastructure.Status
{
	using System;
	using System.Threading.Tasks;

	[Serializable]
	public class AsyncTaskProgress
	{
		public AsyncTaskProgress(Task task)
		{
			TaskId = task.Id;
			Status = task.Status;
			AsyncState = task.AsyncState;
		}

		public object AsyncState { get; set; }

		public int TaskId { get; set; }

		public TaskStatus Status { get; set; }
	}
}